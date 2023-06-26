using DSharpPlus.Entities;
using Eremite.Data.DiscordData;
using Eremite.Services;

namespace Eremite.Actions
{
    public static class TimeGatedAction
    {
        public const string eventAlreadyTriggered = "events.already_triggered";
        public const string triggerTimeSuggestion = "events.trigger_event_timer";
        public const string triggeredKey = "events.triggered";
        public const string eventKey = "events.event_key";
        public const string meetKey = "events.meet";
        public const string collectedKey = "events.collected";
        public const string noCharactersFound = "events.meet0characters";

        /// <summary>
        /// Shows if this event could be ticked (eg. is needed time passed)
        /// </summary>
        /// <param name="timeGatedEvent">Event to check</param>
        /// <returns>returns the possibility of Ticking the event (eg. needed time passed or not)</returns>
        public static bool CheckTimeGatedEvent(TimeGatedEvent timeGatedEvent)
        {
            var dayTickAllowed = timeGatedEvent.LastTimeTriggered.Add(timeGatedEvent.TimeBetweenTriggers); //start date + time needed
            return DateTime.Compare(DateTime.UtcNow, dayTickAllowed) >= 0; //is current time passed this start date + time needed point or not
        }

        public static TimeGatedEvent GetPreviousEventByType(this UserData user, TimeGatedEventType type)
        {
            return user.Events.FirstOrDefault(previousEvent => previousEvent.EventType == type);
        }

        /// <summary>
        /// Ticks the event (u prob want to use <see cref="CheckTimeGatedEvent(TimeGatedEvent)"/> to see if enough time passed or not) <br />
        /// Use it if you want custom login such as checkers n stuff, otherwise easier and safer would be <see cref="HandleEvent(UserData, TimeGatedEvent)"/>
        /// </summary>
        /// <param name="user">User to add award to</param>
        /// <param name="newEvent">Event to get award from and reset timer</param>
        /// <param name="oldEvent">Event to delete (which was used to compare, if it needs to be unique one in the list)</param>
        /// <param name="customAward">If you want to use custom award instead of event one</param>
        public static void TickEvent(this UserData user, TimeGatedEvent newEvent, TimeGatedEvent oldEvent = null)
        {
            var ticks = oldEvent == null ? newEvent.TimesTicked + 1 : oldEvent.TimesTicked + 1;
            newEvent.TimesTicked = ticks;
            newEvent.LastTimeTriggered = DateTime.UtcNow;

            if(oldEvent != null) user.Events.Remove(oldEvent); //remove previous event
            user.Events.Add(newEvent); //add the just-ticked event with updated time


            Console.WriteLine($"[EVENT] {newEvent.EventType} was triggered by the player {user.Username} | {user.UserId}");
            PerkAction.ApplyPerk(user, newEvent.EventType, newEvent.Award);

            user.AddAward(newEvent.Award);
        }

        /// <summary>
        /// Handles checks for event to be ticked and ticks the event if time needed already passed for you <br />
        /// If you want to tick the event no matter the time (manually), use <see cref="TickEvent(UserData, TimeGatedEvent, Award)"/>
        /// </summary>
        /// <param name="user">User who participating in event</param>
        /// <param name="timeGatedEvent">Event to handle</param>
        /// <returns>Embed with result (error text with time or <see cref="GetEventEmbed(UserData, TimeGatedEvent)"/>)</returns>
        public static bool HandleEvent(this UserData user, TimeGatedEvent participatedEvent)
        {
            bool isPossible = true;
            var previousEvent = user.GetPreviousEventByType(participatedEvent.EventType);
            if (previousEvent == null)
            {
                user.TickEvent(participatedEvent);
                return isPossible; //havent triggered any events like this, so user is free to do so
            }

            isPossible = CheckTimeGatedEvent(previousEvent);
            if (!isPossible) return isPossible;

            TickEvent(user, participatedEvent, previousEvent); //tick the current event
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

            string characters = award.CharactersToAdd?.Count > 0 ? award.CharactersToAdd.GetHighestTier().CharacterName : user.GetText(noCharactersFound);
            return new DiscordEmbedBuilder()
            {
                Color = DiscordColor.Purple,
                Title = $"{user.Username} {user.GetText(triggeredKey)} {timeGatedEvent.EventType} {user.GetText(eventKey)}",
                ImageUrl = timeGatedEvent.ImageUrl,
                Description = $"{user.Username} {user.GetText(meetKey)} {characters} {user.GetText(collectedKey)} {award.CurrenciesToAdd}"
            };
        }
    }
}
