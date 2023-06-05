
using Eremite.Data;
using Newtonsoft.Json;
using MySql.Data.MySqlClient;
using Eremite.Data.DiscordData;

namespace Eremite.Services
{
    public class DataHandler
    {
        private DbConfig cachedConfig = null;

        public async Task SendData(UserData userData)
        {
            if (cachedConfig == null) await CacheConfig(); //read connection config

            var connector = new DbConnector(cachedConfig); //open connection
            await connector.ConnectAsync();

            //get corresponding user
            string query = $"SELECT userid, primogems, mora FROM users WHERE userid = {userData.UserId}";
            var selectCommand = new MySqlCommand(query, connector.Connection);

            var user = ReadData(selectCommand);

            if (!user.IsValid()) query = $"INSERT INTO `users`(`userid`, `primogems`, `mora`) VALUES ('{userData.UserId}','{userData.Wallet.Primogems}','{userData.Wallet.Mora}')";
            else query = $"UPDATE users SET primogems = {userData.Wallet.Primogems}, mora = {userData.Wallet.Mora} WHERE userid = {userData.UserId}";

            var updateCommand = new MySqlCommand(query, connector.Connection);
            await updateCommand.ExecuteScalarAsync();

            //close connection
            await connector.CloseAndDisposeAsync();
        }

        public async Task<UserData> GetData(string userId)
        {
            if (cachedConfig == null) await CacheConfig(); //read connection config

            var connector = new DbConnector(cachedConfig); //open connection
            await connector.ConnectAsync();

            //select user from db with matching id
            string query = $"SELECT userid, primogems, mora FROM users WHERE userid = {userId}";
            var command = new MySqlCommand(query, connector.Connection);

            var user = ReadData(command);

            await connector.CloseAndDisposeAsync(); //close connection

            if (!user.IsValid())
            {
                user.UserId = userId;
                await SendData(user);
            }

            return user;
        }

        private UserData ReadData(MySqlCommand command)
        {
            var user = new UserData();
            var reader = command.ExecuteReader();
            while (reader.Read())
            {
                user.UserId = reader.GetString("userid");
                user.AddCurrency(reader.GetInt32("primogems"), reader.GetInt32("mora"));
            }

            reader.Close();
            command.Dispose();
            Console.WriteLine("ReadingData..");
            return user;
        }

        private async Task CacheConfig()
        {
            var rawConfig = await DataRouter.ReadFromConfigs(DbConnector.DbConfig);
            cachedConfig = JsonConvert.DeserializeObject<DbConfig>(rawConfig);
        }
    }
}
