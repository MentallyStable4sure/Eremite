using DSharpPlus.Entities;
using Eremite.Data;
using Eremite.Services;
using Eremite.Data.DiscordData;
using Eremite.Builders;
using Eremite.Base.Interfaces;

namespace Eremite.Actions
{
    public class PullAction : IEremiteService
    {
        public DataHandler DataHandler { get; set; }

        public PullAction(DataHandler dataHandler) => DataHandler = dataHandler;

        private const string wishKey = "pull.wish";

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

            var chances = DataHandler.Config.Chances;

            for (int i = 0; i < numberOfPulls; i++)
            {
                var star = GetStarByChance(chances);
                var charactersPool = GetCharactersPoolByStar(CharactersHandler.CharactersData, star);

                user.Wallet.Primogems -= cost;
                var pulledCharacter = charactersPool[Random.Shared.Next(0, charactersPool.Count)];

                user.AddPulledCharacter(pulledCharacter);
                charactersGot.Add(pulledCharacter);
            }

            user.Stats.TimesPulled += numberOfPulls;
            user.Stats.TotalPrimogemsSpent += numberOfPulls * cost;

            return charactersGot;
        }

        private List<Character> GetCharactersPoolByStar(List<Character> allCharacters, int star)
        {
            return allCharacters.FindAll(character => character.StarsRarity == star);
        }

        //pseudo random on probability
        private int GetStarByChance(Dictionary<int, int> chances)
        {
            var randomPercent = Random.Shared.Next(0, 101);
            int starRarity = 3;

            foreach (var percentage in chances)
            {
                if (randomPercent > percentage.Value) continue;
                if (starRarity > percentage.Key) continue;

                starRarity = percentage.Key;
            }
            
            return starRarity;
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
            if(characters.Count == 0) return characters;

            var updateQuery = new UserUpdateQueryBuilder(user, QueryElement.Characters, QueryElement.Wallet, QueryElement.Stats).Build();

            await DataHandler.SendData(user, updateQuery);

            return characters;
        }

        public static DiscordEmbedBuilder GetEmbedWithCharacters(List<Character> characters, UserData user)
        {
            var highestTier = characters.GetHighestTier();
            var highestTierColor = highestTier.GetCorrespondingColor();

            return new DiscordEmbedBuilder()
            {
                Color = highestTierColor,
                Title = $"{user.Username} {user.GetText(wishKey)}",
                ImageUrl = highestTier.ImagePullBannerUrl,
                Description = $"> {characters.ToCharacterList(user)}"
            };
        }
    }
}
