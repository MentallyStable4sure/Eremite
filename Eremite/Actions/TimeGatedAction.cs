using DSharpPlus.Entities;
using Eremite.Data.DiscordData;

namespace Eremite.Actions
{
    public static class TimeGatedAction
    {
        public const string ErrorByTime = "> You already triggered this event.";

        /// <summary>
        /// Shows if this event could be ticked (eg. is needed time passed)
        /// </summary>
        /// <param name="timeGatedEvent">Event to check</param>
        /// <returns>returns the possibility of Ticking the event (eg. needed time passed or not)</returns>
        public static bool CheckTimeGatedEvent(TimeGatedEvent timeGatedEvent)
        {
            var dayTickAllowed = timeGatedEvent.LastTimeTriggered.Add(timeGatedEvent.TimeBetweenTriggers);
            return DateTime.Compare(timeGatedEvent.LastTimeTriggered, dayTickAllowed) == 1;
        }

        public static TimeGatedEvent GetPreviousEventByType(this UserData user, TimeGatedEventType type)
        {
            return user.Events.FirstOrDefault(previousEvent => previousEvent.EventType == type);
        }

        /// <summary>
        /// Ticks the event (u prob want to use <see cref="CheckTimeGatedEvent(TimeGatedEvent)"/> to see if enough time passed or not)
        /// Use it if you want custom login such as checkers n stuff, otherwise easier and safer would be <see cref="HandleEventTick(UserData, TimeGatedEvent)"/>
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
        /// Handles checks for event to be ticked and ticks the event if time needed already passed for you
        /// If you want to tick the event no matter the time (manually), use <see cref="TickEvent(UserData, TimeGatedEvent, Award)"/>
        /// </summary>
        /// <param name="user">User who participating in event</param>
        /// <param name="timeGatedEvent">Event to handle</param>
        /// <returns>Embed with result (error text with time or <see cref="GetEventEmbed(UserData, TimeGatedEvent)"/>)</returns>
        public static bool HandleEvent(this UserData user, TimeGatedEvent participatedEvent)
        {
            bool isPossible = true;

            var previousEvent = user.GetPreviousEventByType(participatedEvent.EventType);
            if (previousEvent == null) return isPossible; //havent triggered any events like this, so user is free to do so

            isPossible = CheckTimeGatedEvent(previousEvent);
            if (!isPossible) return isPossible;

            user.Events.Remove(previousEvent); //remove previous event
            TickEvent(user, participatedEvent); //tick the current event
            user.Events.Add(participatedEvent); //add the just-ticked event with updated time

            return isPossible;
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
