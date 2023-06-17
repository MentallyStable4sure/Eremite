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
                    $"\n\nTraveled: {user.Stats.TimesTraveled} times | Dailies completed: {user.Stats.TimesDailiesCompleted} times" +
                    $"\nTotal primogems earned/spent: [{user.Stats.TotalPrimogemsEarned}|{user.Stats.TotalPrimogemsSpent}]" +
                    $"\nLargest cashback: {user.Stats.LargestCashback} | Total cashback: {user.Stats.TotalCashback}" +
                    $"\nTotal pills earned/spent: [{user.Stats.TotalPillsEarned}|{user.Stats.TotalPillsSpent}]";
        }

        /// <summary>
        /// Creates an Embed with user stats
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
    }
}
