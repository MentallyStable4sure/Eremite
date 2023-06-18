
using Eremite.Data;
using Eremite.Data.DiscordData;

namespace Eremite.Services
{
    public class CharactersHandler
    {
        public static List<Character> CharactersData { get; protected set; } = new List<Character>();

        public DatabaseConfig DatabaseConfig { get; protected set; }

        public CharactersHandler(DatabaseConfig databaseConfig) => DatabaseConfig = databaseConfig;

        public async Task InitializeCharacterList()
        {
            var connector = new DbConnector(DatabaseConfig); //open connection
            await connector.ConnectAsync();

            //select user from db with matching id
            var characters = QueryHandler.GetCharacterFromQuery(connector, 0);

            await connector.CloseAndDisposeAsync(); //close connection

            CharactersData = characters;
        }

        internal async Task ReInitializeCharacterList() => await InitializeCharacterList();

        public static List<Character> ConvertIds(List<int> characters)
        {
            var charactersConverted = new List<Character>();

            foreach (var character in CharactersData)
            {
                if (!characters.Contains(character.CharacterId)) continue;
                charactersConverted.Add(character);
            }

            return charactersConverted;
        }

        public static Character ConvertId(int id) => CharactersData.FirstOrDefault(character => character.CharacterId == id);
    }
}
