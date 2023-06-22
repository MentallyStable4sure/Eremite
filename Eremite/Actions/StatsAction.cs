using DSharpPlus.Entities;
using Eremite.Services;
using Eremite.Commands;
using Eremite.Data.DiscordData;
using System.Text;

namespace Eremite.Actions
{
    internal class StatsAction
    {
        public const int BestCounter = 5;
        public const string TopImage = "https://raw.githubusercontent.com/MentallyStable4sure/Eremite/main/content/events/topusers.png";

        public const string PrimogemsEmoji = "<:imf2pbtw:1113103136991756328>";
        public const string PillsEmoji = "<:pillwhite:1119700330259693629>";

        /// <summary>
        /// Gets formatted string about all user stats
        /// </summary>
        /// <param name="user">user whos stats to check</param>
        /// <returns>already formatted string</returns>
        public static string GetMessageAboutUser(UserData user)
        {
            var currentCharacter = user.IsAnyCharacterEquipped() ? CharactersHandler.ConvertId(user.EquippedCharacter).CharacterName : AkashaCommand.DefaultNullError;

            return $"[ID:{user.UserId}]\n\n> **Main Character: {currentCharacter}**" +
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

        public static DiscordInteractionResponseBuilder SortUsersInBuilder(List<UserData> users, string title = "TOP users:")
        {
            var stringBuilder = new StringBuilder(string.Empty);
            for (int i = 0; i < users.Count; i++)
            {
                var user = users[i];
                stringBuilder.Append($"\n> [{i+1}] {user.Username} | Pulls: {user.Stats.TimesPulled} | {user.Stats.TotalPrimogemsEarned} {PrimogemsEmoji} | {user.Stats.TotalPillsEarned} {PillsEmoji}");
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
