using DSharpPlus.Entities;
using DSharpPlus.SlashCommands;
using Eremite.Actions;
using Eremite.Builders;
using Eremite.Data;
using Eremite.Data.DiscordData;
using Eremite.Services;
using Newtonsoft.Json;

namespace Eremite.SlashCommands
{
    public sealed class DailyCommand : ApplicationCommandModule
    {
        public DataHandler DataHandler { get; set; }

        public List<TimeGatedEvent> CachedDailies { get; protected set; } = null;

        public const TimeGatedEventType DailyType = TimeGatedEventType.Daily;
        public const string DailyConfigs = "dailies.json";

        [SlashCommand("daily", "Shows current daily commision from Eremite Guild")]
        public async Task ShowDailyTask(InteractionContext context)
        {
            var user = await DataHandler.GetData(context.User);
            new InfoAction(DataHandler, context, user);

            if (CachedDailies == null) await CacheDailies();
            if (CachedDailies == null || CachedDailies.Count <= 0) return;

            var randomDaily = CachedDailies[Random.Shared.Next(0, CachedDailies.Count)];

            var isHandled = user.HandleEvent(DataHandler, randomDaily);
            if (!isHandled)
            {
                var previousEvent = user.GetPreviousEventByType(DailyType);
                string countdown = previousEvent.LastTimeTriggered.Add(previousEvent.TimeBetweenTriggers).Subtract(DateTime.UtcNow).GetNormalTime();
                await context.FollowUpAsync(new DiscordFollowupMessageBuilder().WithContent($"> {user.GetText(TimeGatedAction.eventAlreadyTriggered)}. {user.GetText(TimeGatedAction.triggerTimeSuggestion)} {countdown}"));
                return;
            }

            user.Stats.TimesDailiesCompleted++;
            var updateQuery = new UserUpdateQueryBuilder(user, QueryElement.Wallet, QueryElement.Stats, QueryElement.Events, QueryElement.Characters).Build();
            await DataHandler.SendData(user, updateQuery);
            await context.FollowUpAsync(new DiscordFollowupMessageBuilder().AddEmbed(TimeGatedAction.GetEventEmbed(user, randomDaily)));
        }

        public async Task CacheDailies()
        {
            if (CachedDailies != null || CachedDailies?.Count > 0) return;

            var rawDailies = await DataGrabber.GrabFromConfigs(DailyConfigs);

            rawDailies.LogStatus(DailyConfigs);

            CachedDailies = JsonConvert.DeserializeObject<List<TimeGatedEvent>>(rawDailies);
        }
    }
}
