
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

namespace Eremite.Commands
{
    public sealed class AdventureCommand : BaseCommandModule
    {
        public DataHandler DataHandler { get; set; }

        private List<AdventureEvent> CachedAdventures { get; set; }
        private UserData userWhoUsedCommand = null;
        private Dictionary<AdventureEvent, DiscordButtonComponent> buttons = null;

        public const string AdventuresConfig = "adventures.json";
        public const TimeGatedEventType AdventuresType = TimeGatedEventType.Adventure;
        public const string AdventuresImage = "https://raw.githubusercontent.com/MentallyStable4sure/Eremite/main/content/events/adventure.png";
        public const int ChoicesPerAdventure = 2;

        private AdventureEvent GetRandomAdventure() => CachedAdventures[Random.Shared.Next(0, CachedAdventures.Count)];

        [Command("adventure"), Description("Travel to regions or to a desert with eremites and find mora or akasha knowledge in return")]
        public async Task ShowAdventures(CommandContext context)
        {
            userWhoUsedCommand = await DataHandler.GetData(context.User);
            var previousEvent = TimeGatedAction.GetPreviousEventByType(userWhoUsedCommand, AdventuresType);

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
            var randomAdventures = FillRandomAdventures(ChoicesPerAdventure);
            buttons = CreateButtons(randomAdventures);

            await context.RespondAsync(new DiscordMessageBuilder().WithEmbed(new DiscordEmbedBuilder
            {
                Title = $"{userWhoUsedCommand.Username} starts new adventure..",
                Description = $"{userWhoUsedCommand.Username}'s granpa always said: 'every journey has its final day, dont rush', so {userWhoUsedCommand.Username} thinking about where to go next..",
                ImageUrl = AdventuresImage,
                Color = DiscordColor.Orange
            }).AddComponents(buttons.Values));

            context.Client.ComponentInteractionCreated += InteractionClicked;
        }

        private async Task InteractionClicked(DiscordClient sender, ComponentInteractionCreateEventArgs args)
        {
            if (args.User.Id.ToString() != userWhoUsedCommand.UserId.ToString()) return;
            sender.ComponentInteractionCreated -= InteractionClicked;

            var adventure = buttons.Keys.FirstOrDefault(adventure => adventure.ButtonGuid == args.Id);
            if (adventure == null) return;

            var previousEvent = TimeGatedAction.GetPreviousEventByType(userWhoUsedCommand, AdventuresType);
            await GoOnAdventure(args, userWhoUsedCommand, adventure, previousEvent);
        }

        private async Task Save(UserData user)
        {
            var updateQuery = new QueryBuilder(user, QueryElement.Wallet, QueryElement.Stats, QueryElement.Events, QueryElement.Characters).BuildUpdateQuery();
            await DataHandler.SendData(user, updateQuery);
        }

        private Dictionary<AdventureEvent, DiscordButtonComponent> CreateButtons(List<AdventureEvent> adventures)
        {
            var buttons = new Dictionary<AdventureEvent, DiscordButtonComponent>();

            foreach (var adventure in adventures)
            {
                adventure.ButtonGuid = Guid.NewGuid().ToString();
                var button = new DiscordButtonComponent(ButtonStyle.Secondary, adventure.ButtonGuid, adventure.ButtonText);
                buttons.Add(adventure, button);
            }

            return buttons;
        }

        private async Task GoOnAdventure(ComponentInteractionCreateEventArgs args, UserData user, TimeGatedEvent timeGatedEvent, TimeGatedEvent previousEvent)
        {
            if (timeGatedEvent == null) return;

            user.Stats.TimesTraveled++;
            TimeGatedAction.TickEvent(user, timeGatedEvent, previousEvent);
            await Save(user);

            await args.Interaction.CreateResponseAsync(
                InteractionResponseType.UpdateMessage,
                new DiscordInteractionResponseBuilder().AddEmbed(TimeGatedAction.GetEventEmbed(user, timeGatedEvent)));
        }

        private List<AdventureEvent> FillRandomAdventures(int amount)
        {
            var randomAdventures = new List<AdventureEvent>();
            while (randomAdventures.Count < amount)
            {
                var randomAdventure = GetRandomAdventure();
                while (randomAdventures.Find(adventure => adventure.Region == randomAdventure.Region) != null)
                {
                    randomAdventure = GetRandomAdventure();
                }

                randomAdventures.Add(randomAdventure);
            }
            return randomAdventures;
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
