using DSharpPlus.CommandsNext;
using Eremite.Data.DiscordData;

namespace Eremite.Actions
{
    internal class PullAction
    {
        public const string NotEnoughPrimosError = "> Not enough primogems! Try any activities like teapot, travel, etc. to get primogems.";

        /// <summary>
        /// Pulls characters for X amount of times, and adds it to a user list.
        /// </summary>
        /// <param name="user">User to add characters for</param>
        /// <param name="number">X amount of times to pull</param>
        /// <returns>List of characters got from pull(s)</returns>
        public static List<Character> ForUser(UserData user, int numberOfPulls, int cost = 160)
        {
            var charactersGot = new List<Character>();
            if (user.Wallet.Primogems < cost * numberOfPulls) return charactersGot;

            //TODO:
            //will do the pull calculation
            //charge user
            //add character to a user and return a string which character user got

            return charactersGot;
        }

        /// <summary>
        /// Pulls characters for X amount of times, and adds it to a user list.
        /// Check for user wallet and returns response when error meets
        /// </summary>
        /// <param name="user">User to add characters for</param>
        /// <param name="number">X amount of times to pull</param>
        /// <returns>List of characters got from pull(s)</returns>
        public static async Task<List<Character>> ForUserAsync(CommandContext context, UserData user, int numberOfPulls, int cost = 160)
        {
            var charactersGot = new List<Character>();
            if(user.Wallet.Primogems < cost * numberOfPulls)
            {
                await context.RespondAsync(NotEnoughPrimosError);
                return charactersGot;
            }

            return ForUser(user, numberOfPulls, cost);
        }
    }
}
