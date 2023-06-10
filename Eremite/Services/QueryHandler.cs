using MySql.Data.MySqlClient;
using Eremite.Data.DiscordData;
using Newtonsoft.Json;

namespace Eremite.Services
{
    // TODO: Query builder
    //
    // Make a Query builder with interfaces to drop, each interface will provide GetQueryInfo() method with their strings for update
    // or do like a enums with foreach
    internal class QueryHandler
    {
        /// <summary>
        /// Gets full user data from entire table
        /// </summary>
        /// <returns>user with filled data</returns>
        public static UserData GetUserFromQuery(string userId, DbConnector connector)
        {
            string query = GetSelectUserQuery(userId);
            var selectCommand = new MySqlCommand(query, connector.Connection);

            var user = ReadUserFromQuery(selectCommand);
            selectCommand.Dispose();
            return user;
        }

        public static UserData ReadUserFromQuery(MySqlCommand command)
        {
            var user = new UserData();
            var reader = command.ExecuteReader();

            while (reader.Read())
            {
                user.UserId = reader.GetString("userid");
                user.Username = reader.GetString("username");
                user.Wallet = JsonConvert.DeserializeObject<DiscordWallet>(reader.GetString("wallet"));
                user.Characters = JsonConvert.DeserializeObject<List<Character>>(reader.GetString("characters"));
                user.EquippedCharacter = JsonConvert.DeserializeObject<Character>(reader.GetString("equippedcharacter"));
                user.Stats = JsonConvert.DeserializeObject<Stats>(reader.GetString("stats"));
                user.Events = JsonConvert.DeserializeObject<List<TimeGatedEvent>>(reader.GetString("events"));
            }

            reader.Close();
            return user;
        }

        /// <returns>Query string with full user selection (select all rows)</returns>
        public static string GetSelectUserQuery(string userId)
        {
            return $"SELECT * FROM users WHERE userid = {userId}";
        }

        /// <returns>Query string for insertion for a completely NEW user (insert all rows)</returns>
        public static string GetUserInsertQuery(UserData user)
        {
            string query = $"INSERT INTO `users`(`userid`, `username`, `wallet`, `characters`, `equippedcharacter`, `stats`, `events`) " +
                $"VALUES ('{user.UserId}','{user.Username}','{JsonConvert.SerializeObject(user.Wallet)}','{JsonConvert.SerializeObject(user.Characters)}'," +
                $"'{JsonConvert.SerializeObject(user.EquippedCharacter)}','{JsonConvert.SerializeObject(user.Stats)}','{JsonConvert.SerializeObject(user.Events)}')";

            return query;
        }
    }
}
