using DSharpPlus.Entities;
using Eremite.Data.DiscordData;

namespace Eremite.Actions
{
    public static class TimeGatedAction
    {
        /// <summary>
        /// Shows if this event could be ticked (eg. is needed time passed)
        /// </summary>
        /// <param name="timeGatedEvent">Event to check</param>
        /// <returns>returns the possibility of Ticking the event (eg. needed time passed or not)</returns>
        public static bool CheckTimeGatedEvent(TimeGatedEvent timeGatedEvent)
        {
            var eventTimeNeeded = timeGatedEvent.LastTimeTriggered;
            var dayTickAllowed = eventTimeNeeded.Add(-timeGatedEvent.TimeBetweenTriggers);

            return DateTime.Compare(timeGatedEvent.LastTimeTriggered, dayTickAllowed) == 1;
        }

        /// <summary>
        /// Ticks the event (u prob want to use <see cref="CheckTimeGatedEvent(TimeGatedEvent)"/> to see if enough time passed or not)
        /// </summary>
        /// <param name="user">User to add award to</param>
        /// <param name="timeGatedEvent">Even to get award from and reset timer</param>
        /// <param name="customAward">If you want to use custom award instead of event one</param>
        public static void TickEvent(this UserData user, TimeGatedEvent timeGatedEvent, Award customAward = null)
        {
            timeGatedEvent.TimesTicked++;
            timeGatedEvent.LastTimeTriggered = DateTime.UtcNow;

            timeGatedEvent.Award ??= customAward;

            user.AddAward(timeGatedEvent.Award);
        }

        /// <summary>
        /// Creates an Embed with user stats
        /// </summary>
        /// <param name="avatarUrl">user's avatar</param>
        /// <param name="user">user whos stats to check</param>
        /// <returns>already builded DiscordEmbed component</returns>
        public static DiscordEmbedBuilder GetEventEmbed(UserData user, TimeGatedEvent timeGatedEvent)
        {
            var award = timeGatedEvent.Award;

            string characters = award.CharactersToAdd?.Count > 0 ? award.CharactersToAdd.GetHighestTier().CharacterName : "0 Characters";
            return new DiscordEmbedBuilder()
            {
                Color = DiscordColor.Purple,
                Title = $"{user.Username} triggered {timeGatedEvent.EventType} event",
                ImageUrl = timeGatedEvent.ImageUrl,
                Description = $"{user.Username} found {characters} and collected {award.CurrenciesToAdd}"
            };
        }
    }
}
