using Eremite.Actions;
using Eremite.Data.DiscordData;
using Eremite.Data;
using Eremite.Services;
using Eremite.Base;
using Newtonsoft.Json;
using DSharpPlus.Entities;
using DSharpPlus;
using DSharpPlus.EventArgs;
using DSharpPlus.SlashCommands;

namespace Eremite.SlashCommands
{
    public sealed class AdventureCommand : ApplicationCommandModule
    {
        public DataHandler DataHandler { get; set; }
        public List<AdventureEvent> CachedAdventures { get; private set; } = null;

        //Localization Keys
        private readonly string noAdventuresFound = "adventures.no_adventures_found";
        private readonly string startAdventure = "adventures.start_adventure";
        private readonly string adventureDescription = "adventures.description";

        public const string AdventuresConfig = "adventures.json";
        public const string AdventuresImage = "https://raw.githubusercontent.com/MentallyStable4sure/Eremite/main/content/events/adventure.png";

        [SlashCommand("adventure", "Travel to regions and find rewards or new teammates")]
        public async Task ShowAdventures(InteractionContext context)
        {
            var user = await DataHandler.GetData(context.User);
            new InfoAction(DataHandler, context, user);

            var previousEvent = user.GetPreviousEventByType(AdventureAction.AdventuresType);

            bool isPossible = true;

            var message = new DiscordInteractionResponseBuilder();
            if (previousEvent != null) isPossible = TimeGatedAction.CheckTimeGatedEvent(previousEvent);
            if (!isPossible)
            {
                string countdown = previousEvent.LastTimeTriggered.Add(previousEvent.TimeBetweenTriggers).Subtract(DateTime.UtcNow).GetNormalTime();
                await context.CreateResponseAsync(message.WithContent($"> {user.GetText(TimeGatedAction.eventAlreadyTriggered)}. {user.GetText(TimeGatedAction.triggerTimeSuggestion)} {countdown}"));
                return;
            }

            if (CachedAdventures == null) await CacheAdventures();
            if (CachedAdventures == null || CachedAdventures.Count <= 0)
            {
                await context.CreateResponseAsync(message.WithContent(user.GetText(noAdventuresFound)));
                return;
            }

            var action = new AdventureAction(user, DataHandler);
            var randomAdventures = action.FillRandomAdventures(CachedAdventures);
            var buttons = CreateButtons(randomAdventures);

            await context.CreateResponseAsync(message.AddEmbed(new DiscordEmbedBuilder
            {
                Title = $"{user.Username} {user.GetText(startAdventure)}",
                Description = user.GetText(adventureDescription),
                ImageUrl = AdventuresImage,
                Color = DiscordColor.Orange
            }.Build()).AddComponents(buttons.Values));

            context.Client.ComponentInteractionCreated += async (sender, args) => await InteractionClicked(action, buttons, args);
        }

        private async Task InteractionClicked(AdventureAction action, Dictionary<BaseIdentifier, DiscordButtonComponent> buttons, ComponentInteractionCreateEventArgs args)
        {
            if (args.User.Id.ToString() != action.CurrentUser.UserId) return;
            if (buttons == null || buttons.Keys == null) return;

            bool isPossible = true;

            var adventure = buttons.Keys.FirstOrDefault(adventure => adventure.identifier == args.Id)?.content as AdventureEvent;
            if (adventure == null) return;

            var previousEvent = action.CurrentUser.GetPreviousEventByType(AdventureAction.AdventuresType);
            if (previousEvent != null) isPossible = TimeGatedAction.CheckTimeGatedEvent(previousEvent);
            if (!isPossible) return;

            await action.GoOnAdventure(args, adventure, previousEvent);
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
