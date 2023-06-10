using DSharpPlus.CommandsNext;
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

        public const string DailyConfigs = "dailies.json";

        [Command("daily"), Description("Shows current daily commision from Eremite Guild")]
        public async Task ShowDailyTask(CommandContext context)
        {
            var user = await DataHandler.GetData(context.User);

            if (CachedDailies == null) await CacheDailies(DailyConfigs);
            if (CachedDailies == null || CachedDailies.Count <= 0) return;

            var randomDaily = CachedDailies[Random.Shared.Next(0, CachedDailies.Count)];

            var embed = TimeGatedAction.GetEventEmbed(user, randomDaily);

            var updateQuery = new QueryBuilder(user, QueryElement.Wallet, QueryElement.Events, QueryElement.Stats).BuildUpdateQuery();
            await DataHandler.SendData(user, updateQuery);
            await context.RespondAsync(embed);
        }

        public async Task<List<TimeGatedEvent>> CacheDailies(string configFile)
        {
            if (CachedDailies != null || CachedDailies?.Count > 0) return CachedDailies;

            var rawDailies = await DataGrabber.GrabFromConfigs(configFile);

            rawDailies.LogStatus(DailyConfigs);
            if (rawDailies == null) return null;

            return JsonConvert.DeserializeObject<List<TimeGatedEvent>>(rawDailies);
        }
    }
}
