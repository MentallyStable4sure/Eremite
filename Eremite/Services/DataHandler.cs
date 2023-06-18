
using Eremite.Data;
using Newtonsoft.Json;
using DSharpPlus.Entities;
using MySql.Data.MySqlClient;
using Eremite.Data.DiscordData;
using Eremite.Builders;

namespace Eremite.Services
{
    public class DataHandler
    {
        private DatabaseConfig cachedDbConfig = null;

        public const string ConfigFile = "config.json";
        public const string CharactersDataFile = "characters.json";

        public Config Config { get; protected set; }

        public DataHandler(DatabaseConfig cachedDbConfig)
        {
            this.cachedDbConfig = cachedDbConfig;
            CacheMainConfig().ConfigureAwait(false);
        }

        public async Task SendData(UserData userData, string customQuery = "")
        {
            var connector = new DbConnector(cachedDbConfig); //open connection
            await connector.ConnectAsync();

            //select user from db with matching id
            var user = QueryHandler.GetUserFromQuery(userData.UserId, connector);

            string query = string.Empty;
            if (!user.IsValid()) query = QueryHandler.GetUserInsertQuery(user);
            else query = customQuery != string.Empty ? customQuery : new UserUpdateQueryBuilder(user, QueryElement.All).Build();

            var updateCommand = new MySqlCommand(query, connector.Connection);
            await updateCommand.ExecuteScalarAsync();

            await updateCommand.DisposeAsync();

            //close connection
            await connector.CloseAndDisposeAsync();
        }

        public async Task<UserData> GetData(DiscordUser discordUser)
        {
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


        public async Task CacheMainConfig()
        {
            var rawData = await DataGrabber.GrabFromConfigs(ConfigFile);

            rawData.LogStatus(ConfigFile);

            Config = JsonConvert.DeserializeObject<Config>(rawData);
        }
    }
}
