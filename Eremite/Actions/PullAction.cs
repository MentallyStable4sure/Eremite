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
        public (List<Character>, int) ForUser(UserData user, int numberOfPulls)
        {
            var charactersGot = new List<Character>();
            var cost = DataHandler.Config.PullCost;

            if (user.Wallet.Primogems < cost * numberOfPulls) return (charactersGot, 0);

            var chances = DataHandler.Config.Chances;

            var cashback = new CashbackAction(DataHandler, user);
            int totalCashback = 0;

            for (int i = 0; i < numberOfPulls; i++)
            {
                var star = GetStarByChance(chances);
                var charactersPool = GetCharactersPoolByStar(CharactersHandler.CharactersData, star);

                user.Wallet.Primogems -= cost;
                var pulledCharacter = charactersPool[Random.Shared.Next(0, charactersPool.Count)];

                totalCashback += cashback.GetCashbackForCharacter(pulledCharacter.CharacterId);
                user.AddPulledCharacter(pulledCharacter);
                charactersGot.Add(pulledCharacter);
            }

            user.Wallet.Mora += totalCashback;
            user.Stats.TimesPulled += numberOfPulls;
            user.Stats.TotalPrimogemsSpent += numberOfPulls * cost;

            return (charactersGot, totalCashback);
        }

        public static List<Character> GetCharactersPoolByStar(List<Character> allCharacters, int star)
        {
            return allCharacters.FindAll(character => character.StarsRarity == star);
        }

        //pseudo random on probability
        public static int GetStarByChance(Dictionary<int, int> chances)
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
        public async Task<(List<Character>, int)> ForUserAsyncSave(UserData user, int numberOfPulls)
        {
            var result = ForUser(user, numberOfPulls);
            var characters = result.Item1;
            var totalCashback = result.Item2;

            if(characters.Count <= 0) return (characters, totalCashback);

            var updateQuery = new UserUpdateQueryBuilder(user, QueryElement.Characters, QueryElement.Wallet, QueryElement.Stats).Build();

            await DataHandler.SendData(user, updateQuery);

            return (characters, totalCashback);
        }

        public static DiscordEmbedBuilder GetEmbedWithCharacters(List<Character> characters, string cashback, UserData user)
        {
            var highestTier = characters.GetHighestTier();
            var highestTierColor = highestTier.GetCorrespondingColor();

            return new DiscordEmbedBuilder()
            {
                Color = highestTierColor,
                Title = $"{user.Username} {user.GetText(wishKey)}",
                ImageUrl = highestTier.ImagePullBannerUrl,
                Description = $"> {characters.ToCharacterList(user)}\n{cashback}"
            };
        }
    }
}
