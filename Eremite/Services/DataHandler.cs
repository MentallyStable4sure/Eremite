
using Eremite.Data;
using Newtonsoft.Json;
using MySql.Data.MySqlClient;
using Eremite.Data.DiscordData;

namespace Eremite.Services
{
    public class DataHandler
    {
        private DbConfig cachedConfig = null;

        public async Task SendData(UserData userData, string customQuery = "")
        {
            if (cachedConfig == null) await CacheConfig(); //read connection config

            var connector = new DbConnector(cachedConfig); //open connection
            await connector.ConnectAsync();

            //select user from db with matching id
            var user = QueryHandler.GetUserFromQuery(userData.UserId, connector);

            string query = string.Empty;
            if (!user.IsValid()) query = QueryHandler.GetUserInsertQuery(user);
            else query = customQuery != string.Empty ? customQuery : QueryHandler.GetUserUpdateAll(userData);

            var updateCommand = new MySqlCommand(query, connector.Connection);
            await updateCommand.ExecuteScalarAsync();

            await updateCommand.DisposeAsync();

            //close connection
            await connector.CloseAndDisposeAsync();
        }

        public async Task<UserData> GetData(string userId)
        {
            if (cachedConfig == null) await CacheConfig(); //read connection config

            var connector = new DbConnector(cachedConfig); //open connection
            await connector.ConnectAsync();

            //select user from db with matching id
            var user = QueryHandler.GetUserFromQuery(userId, connector);


            if (!user.IsValid())
            {
                user = new UserData();
                user.UserId = userId;

                await SendDataCustomQuery(QueryHandler.GetUserInsertQuery(user), connector);
            }

            await connector.CloseAndDisposeAsync(); //close connection

            return user;
        }

        private async Task SendDataCustomQuery(string customQuery, DbConnector connector)
        {
            var updateCommand = new MySqlCommand(customQuery, connector.Connection);
            await updateCommand.ExecuteScalarAsync();
        }

        private async Task CacheConfig()
        {
            var rawConfig = await DataRouter.ReadFromConfigs(DbConnector.DbConfig);
            cachedConfig = JsonConvert.DeserializeObject<DbConfig>(rawConfig);
        }
    }
}
