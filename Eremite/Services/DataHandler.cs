
using Eremite.Data;
using Newtonsoft.Json;
using DSharpPlus.Entities;
using MySql.Data.MySqlClient;
using Eremite.Data.DiscordData;

namespace Eremite.Services
{
    public class DataHandler
    {
        private DatabaseConfig cachedDbConfig = null;

        public const string ConfigFile = "config.json";

        public Config Config { get; protected set; }

        public DataHandler() => CacheMainConfig();

        public async Task SendData(UserData userData, string customQuery = "")
        {
            if (cachedDbConfig == null) await CacheDatabaseConfig(); //read connection config

            var connector = new DbConnector(cachedDbConfig); //open connection
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

        public async Task<UserData> GetData(DiscordUser discordUser)
        {
            if (cachedDbConfig == null) await CacheDatabaseConfig(); //read connection config

            var connector = new DbConnector(cachedDbConfig); //open connection
            await connector.ConnectAsync();

            //select user from db with matching id
            var user = QueryHandler.GetUserFromQuery(discordUser.Id.ToString(), connector);


            if (!user.IsValid())
            {
                user = new UserData();
                user.Username = discordUser.Username;
                user.UserId = discordUser.Id.ToString();

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

        private async Task CacheDatabaseConfig()
        {
            var rawConfig = await DataGrabber.GrabFromConfigs(DbConnector.DbConfig);

            if (rawConfig == null)
            {
                Console.WriteLine($"[ERROR] While loading {DbConnector.DbConfig}");
                return;
            }

            cachedDbConfig = JsonConvert.DeserializeObject<DatabaseConfig>(rawConfig);
            Console.WriteLine($"[SUCCESS] {DbConnector.DbConfig} loaded");
        }


        public async void CacheMainConfig()
        {
            var rawData = await DataGrabber.GrabFromConfigs(ConfigFile);

            if (rawData == null)
            {
                Console.WriteLine($"[ERROR] While loading {ConfigFile}");
                return;
            }

            Config = JsonConvert.DeserializeObject<Config>(rawData);
            Console.WriteLine($"[SUCCESS] {ConfigFile} loaded");
        }
    }
}
