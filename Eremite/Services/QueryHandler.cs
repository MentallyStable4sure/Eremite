using MySql.Data.MySqlClient;
using Eremite.Data.DiscordData;
using Newtonsoft.Json;
using Eremite.Builders;
using Eremite.Base.Interfaces;

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
                user.Characters = JsonConvert.DeserializeObject<List<int>>(reader.GetString("characters"));
                user.EquippedCharacter = reader.GetInt32("equippedcharacter");
                user.Stats = JsonConvert.DeserializeObject<Stats>(reader.GetString("stats"));
                user.Events = JsonConvert.DeserializeObject<List<TimeGatedEvent>>(reader.GetString("events"));
            }

            reader.Close();
            return user;
        }

        public static List<Character> ReadCharactersFromQuery(MySqlCommand command)
        {
            var characters = new List<Character>();

            var reader = command.ExecuteReader();

            while (reader.Read())
            {
                var character = new Character();

                character.CharacterName = reader.GetString("CharacterName");
                character.CharacterId = reader.GetInt32("CharacterId");
                character.StarsRarity = reader.GetInt32("StarsRarity");
                character.ImageAkashaBannerUrl = reader.GetString("ImageAkashaBannerUrl");
                character.ImagePullBannerUrl = reader.GetString("ImagePullBannerUrl");
                character.PerkStat = reader.GetInt32("PerkStat");
                character.PerkInfo = reader.GetString("PerkInfo");
                character.ShouldBeDestroyed = reader.GetBoolean("ShouldBeDestroyed");
                character.SellPrice = reader.GetInt32("SellPrice");

                characters.Add(character);
            }

            reader.Close();
            return characters;
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
                $"'{user.EquippedCharacter}','{JsonConvert.SerializeObject(user.Stats)}','{JsonConvert.SerializeObject(user.Events)}')";

            return query;
        }

        /// <summary>
        /// Gets the character from remote table with the proper ID (0 if u want all of the characters)
        /// </summary>
        /// <param name="charactersId">ID of a characters to get (put 0 to get everyone)</param>
        /// <returns>A list of characters filled with info (e.g list of <see cref="Character"/>s)</returns>
        public static List<Character> GetCharacterFromQuery(DbConnector connector, params int[] charactersId)
        {
            if (charactersId.Contains(0)) return GetAllCharacters(connector);

            string query = new CharactersQueryBuilder(charactersId).Build();
            var selectCommand = new MySqlCommand(query, connector.Connection);

            var characters = ReadCharactersFromQuery(selectCommand);
            selectCommand.Dispose();
            return characters;
        }

        private static List<Character> GetAllCharacters(DbConnector connector)
        {
            string query = new CharactersQueryBuilder(0).Build();
            var selectCommand = new MySqlCommand(query, connector.Connection);

            var characters = ReadCharactersFromQuery(selectCommand);
            selectCommand.Dispose();
            return characters;
        }
    }
}
