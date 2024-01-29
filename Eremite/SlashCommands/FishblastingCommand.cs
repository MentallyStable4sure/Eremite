using DSharpPlus;
using DSharpPlus.Entities;
using DSharpPlus.EventArgs;
using DSharpPlus.SlashCommands;
using Eremite.Actions;
using Eremite.Base;
using Eremite.Data;
using Eremite.Data.DiscordData;
using Eremite.Services;
using Newtonsoft.Json;

namespace Eremite.SlashCommands
{
    public sealed class FishblastingCommand : ApplicationCommandModule
    {
        public DataHandler DataHandler { get; set; }

        public List<FishblastingEvent> CachedEvents { get; protected set; } = null;

        public const string FishblastingConfigs = "fishblasting.json";

        public const string BaseFishblastingImage = "https://raw.githubusercontent.com/MentallyStable4sure/Eremite/main/content/events/fishblasting.jpg";

        [SlashCommand("fishblasting", "Go to a fishing spot with Klee")]
        public async Task ShowFishblasting(InteractionContext context)
        {
            var currentUser = await DataHandler.GetData(context.User);
            new InfoAction(DataHandler, context, currentUser);
            var message = new DiscordInteractionResponseBuilder();

            if (CachedEvents == null) await CacheFishblasting();
            if (CachedEvents == null || CachedEvents.Count <= 0) return;

            var action = new FishblastingAction(currentUser, DataHandler);

            bool isPossible = true;
            var previousEvent = currentUser.GetPreviousEventByType(FishblastingAction.FishblastingType);
            if (previousEvent != null) isPossible = TimeGatedAction.CheckTimeGatedEvent(previousEvent);
            if (!isPossible)
            {
                string countdown = previousEvent.LastTimeTriggered.Add(previousEvent.TimeBetweenTriggers).Subtract(DateTime.UtcNow).GetNormalTime();
                await context.CreateResponseAsync(message.WithContent($"> {currentUser.GetText(TimeGatedAction.eventAlreadyTriggered)}. {currentUser.GetText(TimeGatedAction.triggerTimeSuggestion)} {countdown}"));
                return;
            }

            var randomList = action.FillRandomSpots(CachedEvents, 2);
            var buttons = CreateButtons(randomList);
            await context.CreateResponseAsync(message.AddEmbed(new DiscordEmbedBuilder
            {
                Title = $"🐟🐡🐠",
                Description = "> Choose a spot! Dont forget to /equip the fishing rod. If you dont know rod id check out your /inventory.",
                ImageUrl = BaseFishblastingImage,
                Color = DiscordColor.Cyan
            }.Build()).AddComponents(buttons.Values));

            context.Client.ComponentInteractionCreated += async (sender, args) => await InteractionClicked(action, buttons, args);
        }

        private async Task InteractionClicked(FishblastingAction action, Dictionary<BaseIdentifier, DiscordButtonComponent> buttons, ComponentInteractionCreateEventArgs args)
        {
            if (args.User.Id.ToString() != action.CurrentUser.UserId) return;
            if (buttons == null || buttons.Keys == null) return;
            bool isPossible = true;

            var fishSpotToGo = buttons.Keys.FirstOrDefault(adventure => adventure.identifier == args.Id)?.content as FishblastingEvent;
            if (fishSpotToGo == null) return;

            var previousEvent = action.CurrentUser.GetPreviousEventByType(FishblastingAction.FishblastingType);
            if (previousEvent != null) isPossible = TimeGatedAction.CheckTimeGatedEvent(previousEvent);
            if (!isPossible) return;

            await action.GoOnFishing(args, fishSpotToGo, previousEvent);
        }

        private Dictionary<BaseIdentifier, DiscordButtonComponent> CreateButtons(List<FishblastingEvent> spots)
        {
            var buttons = new Dictionary<BaseIdentifier, DiscordButtonComponent>();

            foreach (var spot in spots)
            {
                var guid = Guid.NewGuid().ToString();
                var identifier = new BaseIdentifier(guid, spot);
                spot.ButtonGuid = guid.ToString();
                var button = new DiscordButtonComponent(ButtonStyle.Secondary, spot.ButtonGuid, spot.ButtonText);
                buttons.Add(identifier, button);
            }

            return buttons;
        }

        public async Task CacheFishblasting()
        {
            if (CachedEvents != null || CachedEvents?.Count > 0) return;

            var rawDailies = await DataGrabber.GrabFromConfigs(FishblastingConfigs);

            rawDailies.LogStatus(FishblastingConfigs);

            CachedEvents = JsonConvert.DeserializeObject<List<FishblastingEvent>>(rawDailies);
        }
    }
}
