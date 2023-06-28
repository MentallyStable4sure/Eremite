using DSharpPlus.Entities;
using Eremite.Services;
using Eremite.Data.DiscordData;
using System.Text;
using Eremite.Layouts;
using Eremite.Data.Localization;

namespace Eremite.Actions
{
    internal class StatsAction
    {
        public const int BestCounter = 5;
        public const string TopImage = "https://raw.githubusercontent.com/MentallyStable4sure/Eremite/main/content/events/topusers.png";

        public const string timesPulled = "stats.times_pulled"; //Times pulled
        public const string timesTraveled = "stats.times_traveled"; //Times traveled
        public const string dailiesCompleted = "stats.dailies_completed"; //Dailies completed
        public const string totalPirmogems = "stats.total_primogems"; //Total primogems earned/spent
        public const string totalPills = "stats.total_pills"; //Total pills earned/spent

        /// <summary>
        /// Gets formatted string about all user stats
        /// </summary>
        /// <param name="user">user whos stats to check</param>
        /// <returns>already formatted string</returns>
        public static string GetMessageAboutUser(UserData user)
        {
            var currentCharacter = user.IsAnyCharacterEquipped() ? user.GetText($"character.{user.EquippedCharacter}.name") : user.GetText(SetCharacterAction.noMainCharacter);

            return $"[ID:{user.UserId}]\n\n> **{user.GetText(AkashaLayout.mainCharacterKey)} {currentCharacter}**" +
                    $"\n> {user.GetText(AkashaLayout.charactersObtained)} {user.Characters.Count} | {user.GetText(timesPulled)} {user.Stats.TimesPulled}" +
                    $"\n\n{user.GetText(timesTraveled)} {user.Stats.TimesTraveled} | {user.GetText(dailiesCompleted)} {user.Stats.TimesDailiesCompleted}" +
                    $"\n{user.GetText(totalPirmogems)} [{user.Stats.TotalPrimogemsEarned}|{user.Stats.TotalPrimogemsSpent}]" +
                    $"\n{user.GetText(totalPills)} [{user.Stats.TotalPillsEarned}|{user.Stats.TotalPillsSpent}]";
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

        public static async Task<List<UserData>> GetTopUsers(DataHandler dataHandler, SortMethod sortMethod, int bestOf = BestCounter)
        {
            var allUsers = await dataHandler.GetAllUsers();

            UserData[] usersOrdered = new UserData[0];
            List<UserData> topUsers = new List<UserData>(bestOf);

            switch (sortMethod)
            {
                case SortMethod.Pulls:
                    usersOrdered = allUsers.OrderByDescending(user => user.Stats.TimesPulled).ToArray();
                    break;

                case SortMethod.Primogems:
                    usersOrdered = allUsers.OrderByDescending(user => user.Stats.TotalPrimogemsEarned).ToArray();
                    break;

                case SortMethod.Pills:
                    usersOrdered = allUsers.OrderByDescending(user => user.Stats.TotalPillsEarned).ToArray();
                    break;

                case SortMethod.Adventure:
                    usersOrdered = allUsers.OrderByDescending(user => user.Stats.TimesTraveled).ToArray();
                    break;

                case SortMethod.Daily:
                    usersOrdered = allUsers.OrderByDescending(user => user.Stats.TimesDailiesCompleted).ToArray();
                    break;

                default:
                    return usersOrdered.ToList();
            }

            for (int i = 0; i < bestOf; i++)
            {
                topUsers.Add(usersOrdered[i]);
            }

            return topUsers;
        }

        public static DiscordInteractionResponseBuilder SortUsersInBuilder(Language lang, List<UserData> users, string title = "TOP users:")
        {
            var stringBuilder = new StringBuilder(string.Empty);
            for (int i = 0; i < users.Count; i++)
            {
                var user = users[i];
                stringBuilder.Append($"\n> [{i+1}] {user.Username} | {Localization.GetText(lang, timesPulled)} {user.Stats.TimesPulled} | {user.Stats.TotalPrimogemsEarned} {Localization.PrimosEmoji} | {user.Stats.TotalPillsEarned} {Localization.PillsEmoji}");
            }

            return new DiscordInteractionResponseBuilder().AddEmbed(new DiscordEmbedBuilder()
            {
                Color = DiscordColor.Gold,
                Title = title,
                ImageUrl = TopImage,
                Description = stringBuilder.ToString()
            });
        }
    }
}
