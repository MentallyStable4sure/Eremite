using DSharpPlus.CommandsNext;
using Eremite.Data.DiscordData;
using Eremite.Services;

namespace Eremite.Actions
{
    public class PullAction
    {
        public DataHandler DataHandler { get; set; }

        public const string NotEnoughPrimosError = "> Not enough primogems! Try any activities like teapot, travel, etc. to get primogems.";

        public PullAction(DataHandler dataHandler) => DataHandler = dataHandler;

        /// <summary>
        /// Pulls characters for X amount of times, and adds it to a user list.
        /// </summary>
        /// <param name="user">User to add characters for</param>
        /// <param name="numberOfPulls">X amount of times to pull</param>
        /// <returns>List of characters got from pull(s)</returns>
        public List<Character> ForUser(UserData user, int numberOfPulls)
        {
            var charactersGot = new List<Character>();
            var cost = DataHandler.Config.PullCost;

            if (user.Wallet.Primogems < cost * numberOfPulls) return charactersGot;

            var charactersToPull = DataHandler.CharactersData;

            for (int i = 0; i < numberOfPulls; i++)
            {
                user.Wallet.Primogems -= cost;
                var pulledCharacter = charactersToPull[Random.Shared.Next(0, charactersToPull.Count)];

                user.AddPulledCharacter(pulledCharacter);
                charactersGot.Add(pulledCharacter);
            }

            return charactersGot;
        }

        /// <summary>
        /// Same Pull as Pull.ForUser but async with SaveData on the server after user update
        /// </summary>
        /// <param name="user">User to add characters for</param>
        /// <param name="numberOfPulls">X amount of times to pull</param>
        /// <returns>List of characters got from pull(s)</returns>
        public async Task<List<Character>> ForUserAsyncSave(UserData user, int numberOfPulls)
        {
            var characters = ForUser(user, numberOfPulls);
            await DataHandler.SendData(user, QueryHandler.GetUserUpdateCharactersAndWalletQuery(user));

            return characters;
        }
    }
}
