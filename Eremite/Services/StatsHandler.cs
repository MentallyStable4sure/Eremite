using DSharpPlus.Entities;
using Eremite.Commands;
using Eremite.Data.DiscordData;

namespace Eremite.Services
{
    public class StatsHandler
    {
        public static DiscordEmbedBuilder GetEmbedWithStats(DiscordUser discordUser, UserData user)
        {
            var equippedCharacter = user.EquippedCharacter == null ? Akasha.DefaultNullError : user.EquippedCharacter.CharacterName;

            return new DiscordEmbedBuilder()
            {
                Color = DiscordColor.Orange,
                Title = $"{user.Username}'s stats",
                ImageUrl = discordUser.AvatarUrl,
                Description = $"[ID:{user.UserId}]\n\n> **Main Character: {equippedCharacter}**" +
                    $"\n> Characters pulled: {user.Characters.Count} | Pulled: {user.Stats.TimesPulled} times" +
                    $"\n\nTraveled: {user.Stats.TimesTraveled} times | Teapot visited: {user.Stats.TimesTeapotVisited} times" +
                    $"\nTotal primogems earned/spent: [{user.Stats.TotalPrimogemsEarned}|{user.Stats.TotalPrimogemsSpent}]" +
                    $"\nLargest cashback: {user.Stats.LargestCashback} | Total cashback: {user.Stats.TotalCashback}" +
                    $"\nEnrolled in ERS: {user.Stats.TimesEremitesRecruitSystemEnrolled} times | Welkin Won: {user.Stats.TimesWelkinWon} times"
            };
        }
    }
}
