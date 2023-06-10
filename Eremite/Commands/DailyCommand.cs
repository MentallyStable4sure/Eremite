﻿using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;
using Eremite.Actions;
using Eremite.Data;
using Eremite.Data.DiscordData;
using Eremite.Services;
using Newtonsoft.Json;

namespace Eremite.Commands
{
    public sealed class DailyCommand : BaseCommandModule
    {
        public DataHandler DataHandler { get; set; }

        public List<TimeGatedEvent> CachedDailies { get; protected set; } = null;

        public const TimeGatedEventType DailyType = TimeGatedEventType.Daily;
        public const string DailyConfigs = "dailies.json";

        [Command("daily"), Description("Shows current daily commision from Eremite Guild")]
        public async Task ShowDailyTask(CommandContext context)
        {
            var user = await DataHandler.GetData(context.User);

            if (CachedDailies == null) await CacheDailies();
            if (CachedDailies == null || CachedDailies.Count <= 0) return;

            var randomDaily = CachedDailies[Random.Shared.Next(0, CachedDailies.Count)];

            var isHandled = user.HandleEvent(randomDaily);
            if(!isHandled)
            {
                var previousEvent = user.GetPreviousEventByType(DailyType);
                string countdown = previousEvent.LastTimeTriggered.Add(previousEvent.TimeBetweenTriggers).Subtract(DateTime.UtcNow).GetNormalTime();
                await context.RespondAsync($"> {TimeGatedAction.ErrorByTime}. You can trigger event in {countdown}");
                return;
            }

            var updateQuery = new QueryBuilder(user, QueryElement.Wallet, QueryElement.Stats, QueryElement.Events).BuildUpdateQuery();
            await DataHandler.SendData(user, updateQuery);
            await context.RespondAsync(TimeGatedAction.GetEventEmbed(user, randomDaily));
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