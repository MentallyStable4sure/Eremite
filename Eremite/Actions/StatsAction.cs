using DSharpPlus.Entities;
using Eremite.Commands;
using Eremite.Data.DiscordData;

namespace Eremite.Actions
{
    internal class StatsAction
    {
        /// <summary>
        /// Gets formatted string about all user stats
        /// </summary>
        /// <param name="user">user whos stats to check</param>
        /// <returns>already formatted string</returns>
        public static string GetMessageAboutUser(UserData user)
        {
            var equippedCharacter = user.EquippedCharacter == null ? AkashaCommand.DefaultNullError : user.EquippedCharacter.CharacterName;

            return $"[ID:{user.UserId}]\n\n> **Main Character: {equippedCharacter}**" +
                    $"\n> Characters pulled: {user.Characters.Count} | Pulled: {user.Stats.TimesPulled} times" +
                    $"\n\nTraveled: {user.Stats.TimesTraveled} times | Teapot visited: {user.Stats.TimesTeapotVisited} times" +
                    $"\nTotal primogems earned/spent: [{user.Stats.TotalPrimogemsEarned}|{user.Stats.TotalPrimogemsSpent}]" +
                    $"\nLargest cashback: {user.Stats.LargestCashback} | Total cashback: {user.Stats.TotalCashback}" +
                    $"\nEnrolled in ERS: {user.Stats.TimesEremitesRecruitSystemEnrolled} times | Welkin Won: {user.Stats.TimesWelkinWon} times";
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="avatarUrl">user's avatar</param>
        /// <param name="user">user whos stats to check</param>
        /// <returns>already builded DiscordEmbed component</returns>
        public static DiscordEmbedBuilder GetEmbedWithStats(string avatarUrl, UserData user)
        {
            return new DiscordEmbedBuilder()
            {
                Color = DiscordColor.Orange,
                Title = $"{user.Username}'s stats",
                ImageUrl = avatarUrl,
                Description = GetMessageAboutUser(user)
            };
        }

        public static DiscordEmbedBuilder GetEmbedWithCharacters(List<Character> characters, UserData user)
        {
            var highestTier = characters.GetHighestTier();
            var highestTierColor = highestTier.GetCorrespondingColor();

            return new DiscordEmbedBuilder()
            {
                Color = highestTierColor,
                Title = $"{user.Username} wished for a character...",
                ImageUrl = highestTier.ImagePullBannerUrl,
                Description = $"> {characters.ToCharacterList()}"
            };
        }
    }
}
