using System.Text.Json;
using MySql.Data.MySqlClient;
using Eremite.Data.DiscordData;

namespace Eremite.Services
{
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
                user.Wallet = JsonSerializer.Deserialize<DiscordWallet>(reader.GetString("wallet"));
                user.Characters = JsonSerializer.Deserialize<List<Character>>(reader.GetString("characters"));
                user.EquippedCharacter = JsonSerializer.Deserialize<Character>(reader.GetString("equippedcharacter"));
                user.Stats = JsonSerializer.Deserialize<Stats>(reader.GetString("stats"));
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
            string query = $"INSERT INTO `users`(`userid`, `username`, `wallet`, `characters`, `equippedcharacter`, `stats`) " +
                $"VALUES ('{user.UserId}','{user.Username}','{JsonSerializer.Serialize(user.Wallet)}','{JsonSerializer.Serialize(user.Characters)}'," +
                $"'{JsonSerializer.Serialize(user.EquippedCharacter)}','{JsonSerializer.Serialize(user.Stats)}')";

            return query;
        }

        /// <returns>Query string with only wallet being updated</returns>
        public static string GetUserUpdateWalletQuery(UserData user)
        {
            return $"UPDATE `users` SET `wallet`='{JsonSerializer.Serialize(user.Wallet)}' WHERE `userid`='{user.UserId}'";
        }

        /// <returns>Query string with only characters and equipped character being updated</returns>
        public static string GetUserUpdateCharactersQuery(UserData user)
        {
            return $"UPDATE `users` SET `characters`='{JsonSerializer.Serialize(user.Characters)}',`equippedcharacter`='{JsonSerializer.Serialize(user.EquippedCharacter)}' WHERE `userid`='{user.UserId}'";
        }

        /// <returns>Query string with only characters and equipped character being updated</returns>
        public static string GetUserUpdateCharactersAndWalletQuery(UserData user)
        {
            return $"UPDATE `users` SET `characters`='{JsonSerializer.Serialize(user.Characters)}',`equippedcharacter`='{JsonSerializer.Serialize(user.EquippedCharacter)}',`wallet`='{JsonSerializer.Serialize(user.Wallet)}' WHERE `userid`='{user.UserId}'";
        }

        /// <returns>Query string with only stats being updated</returns>
        public static string GetUserUpdateStatsQuery(UserData user)
        {
            return $"UPDATE `users` SET `stats`='{JsonSerializer.Serialize(user.Stats)}' WHERE userid = {user.UserId}";
        }

        /// <returns>Query string with all possibles user tables to update</returns>
        internal static string GetUserUpdateAll(UserData user)
        {
            string query = $"UPDATE `users` SET `username`='{user.Username}', `wallet`='{JsonSerializer.Serialize(user.Wallet)}',`characters`='{JsonSerializer.Serialize(user.Characters)}'," +
                $"`equippedcharacter`='{JsonSerializer.Serialize(user.EquippedCharacter)}',`stats`='{JsonSerializer.Serialize(user.Stats)}' WHERE userid = {user.UserId}";

            return query;
        }
    }
}
