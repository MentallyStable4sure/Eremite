
using Eremite.Data;
using Newtonsoft.Json;
using MySql.Data.MySqlClient;
using Eremite.Data.DiscordData;

namespace Eremite.Services
{
    public class DataRouter
    {
        private DbConfig cachedConfig = null;

        public async Task SendData(UserData userData)
        {
            //read connection config
            if (cachedConfig == null) await CacheConfig();

            //open connection
            var connector = new DbConnector(cachedConfig);
            await connector.ConnectAsync();

            //send data
            string query = $"SELECT userid, primogems, mora FROM users WHERE userid = {userData.UserId}";
            var cmd = new MySqlCommand(query, connector.Connection);

            var reader = cmd.ExecuteReader();
            var id = reader.GetString("userId");

            if (id == null) query = $"INSERT INTO `users`(`userid`, `primogems`, `mora`) VALUES ('{userData.UserId}','{userData.Wallet.Primogems}','{userData.Wallet.Mora}')";
            else query = $"UPDATE users SET primogems = {userData.Wallet.Primogems}, mora = {userData.Wallet.Mora} WHERE userid = {userData.UserId}";

            cmd = new MySqlCommand(query, connector.Connection);
            await cmd.ExecuteScalarAsync();

            //close connection
            await connector.DisposeAsync();
        }

        public async Task<UserData> GetData(string userId)
        {
            //read connection config
            if (cachedConfig == null) await CacheConfig();

            //open connection
            var connector = new DbConnector(cachedConfig);
            await connector.ConnectAsync();

            //receive data
            string query = $"SELECT userid, primogems, mora FROM users WHERE userid = {userId}";
            var cmd = new MySqlCommand(query, connector.Connection);

            int primosSaved = 0;
            int moraSaved = 0;
            string id = string.Empty;

            var reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                id = reader.GetString("userid");
                primosSaved = reader.GetInt32("primogems");
                moraSaved = reader.GetInt32("mora");

                Console.WriteLine($"UID Found: {id} |  Primos: {primosSaved} | Mora: {moraSaved}");
            }

            //close connection
            await connector.DisposeAsync();

            var user = new UserData();
            user.UserId = id;
            user.AddCurrency(primosSaved, moraSaved);

            if (id == string.Empty || id == null)
            {
                user.UserId = userId;
                user.ResetWallet();
                await SendData(user);
            }

            return user;
        }

        private async Task CacheConfig()
        {
            var rawConfig = await DataHandler.ReadFromConfigs(DbConnector.DbConfig);
            cachedConfig = JsonConvert.DeserializeObject<DbConfig>(rawConfig);
        }
    }
}
