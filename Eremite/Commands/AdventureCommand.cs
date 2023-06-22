
using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;
using Eremite.Actions;
using Eremite.Data.DiscordData;
using Eremite.Data;
using Eremite.Services;
using Newtonsoft.Json;
using DSharpPlus.Entities;
using DSharpPlus;
using DSharpPlus.EventArgs;
using Eremite.Base;

namespace Eremite.Commands
{
    public sealed class AdventureCommand : BaseCommandModule
    {
        public DataHandler DataHandler { get; set; }
        public List<AdventureEvent> CachedAdventures { get; private set; } = null;

        public const string AdventuresConfig = "adventures.json";
        public const string AdventuresImage = "https://raw.githubusercontent.com/MentallyStable4sure/Eremite/main/content/events/adventure.png";

        [Command("adventure"), Description("Travel to regions or to a desert with eremites and find mora or akasha knowledge in return")]
        public async Task ShowAdventures(CommandContext context)
        {
            var user = await DataHandler.GetData(context.User);

            var previousEvent = TimeGatedAction.GetPreviousEventByType(user, AdventureAction.AdventuresType);

            bool isPossible = true;

            if (previousEvent != null) isPossible = TimeGatedAction.CheckTimeGatedEvent(previousEvent);
            if (!isPossible)
            {
                string countdown = previousEvent.LastTimeTriggered.Add(previousEvent.TimeBetweenTriggers).Subtract(DateTime.UtcNow).GetNormalTime();
                await context.RespondAsync($"> {TimeGatedAction.ErrorByTime}. You can trigger event in {countdown}");
                return;
            }

            if (CachedAdventures == null) await CacheAdventures();
            if (CachedAdventures == null || CachedAdventures.Count <= 0)
            {
                await context.RespondAsync("We dont have places to travel, configure places in adventures.json");
                return;
            }
            var randomAdventures = AdventureAction.FillRandomAdventures(CachedAdventures);
            var buttons = CreateButtons(randomAdventures);

            await context.RespondAsync(new DiscordMessageBuilder().WithEmbed(new DiscordEmbedBuilder
            {
                Title = $"{user.Username} starts new adventure..",
                Description = $"{user.Username}'s granpa always said: 'every journey has its final day, dont rush', so {user.Username} thinking about where to go next..",
                ImageUrl = AdventuresImage,
                Color = DiscordColor.Orange
            }).AddComponents(buttons.Values));

            context.Client.ComponentInteractionCreated += async (sender, args) => await InteractionClicked(user, buttons, args);
        }

        private async Task InteractionClicked(UserData user, Dictionary<BaseIdentifier, DiscordButtonComponent> buttons, ComponentInteractionCreateEventArgs args)
        {
            if (args.User.Id.ToString() != user.UserId) return;

            if (buttons == null || buttons.Keys == null) return;
            var adventure = buttons.Keys.FirstOrDefault(adventure => adventure.identifier == args.Id)?.content as AdventureEvent;
            if (adventure == null) return;
            
            var previousEvent = TimeGatedAction.GetPreviousEventByType(user, AdventureAction.AdventuresType);
            await AdventureAction.GoOnAdventure(DataHandler, args, user, adventure, previousEvent);
        }

        private Dictionary<BaseIdentifier, DiscordButtonComponent> CreateButtons(List<AdventureEvent> adventures)
        {
            var buttons = new Dictionary<BaseIdentifier, DiscordButtonComponent>();

            foreach (var adventure in adventures)
            {
                var guid = Guid.NewGuid().ToString();
                var identifier = new BaseIdentifier(guid, adventure);
                adventure.ButtonGuid = guid.ToString();
                var button = new DiscordButtonComponent(ButtonStyle.Secondary, adventure.ButtonGuid, adventure.ButtonText);
                buttons.Add(identifier, button);
            }

            return buttons;
        }

        private async Task CacheAdventures()
        {
            if (CachedAdventures != null || CachedAdventures?.Count > 0) return;

            var rawDailies = await DataGrabber.GrabFromConfigs(AdventuresConfig);

            rawDailies.LogStatus(AdventuresConfig);

            CachedAdventures = JsonConvert.DeserializeObject<List<AdventureEvent>>(rawDailies);
        }
    }
}
