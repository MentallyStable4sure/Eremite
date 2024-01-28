using DSharpPlus.Entities;
using DSharpPlus.EventArgs;
using DSharpPlus;
using Eremite.Data.DiscordData;
using Eremite.Data;
using Eremite.Services;
using Eremite.Builders;

namespace Eremite.Actions
{
    public class FishblastingAction
    {
        public const TimeGatedEventType AdventuresType = TimeGatedEventType.Fishblasting;

        public UserData CurrentUser { get; private set; }

        private readonly DataHandler cachedHandler;

        public FishblastingAction(UserData currentUser, DataHandler cachedHandler)
        {
            CurrentUser = currentUser;
            this.cachedHandler = cachedHandler;
        }

        /*public AdventureEvent GetRandomAdventure(List<AdventureEvent> adventures) => adventures[Random.Shared.Next(0, adventures.Count)];

        public List<AdventureEvent> FillRandomAdventures(List<AdventureEvent> adventuresPool, int amount)
        {
            var randomAdventures = new List<AdventureEvent>();
            while (randomAdventures.Count < amount)
            {
                var randomAdventure = GetRandomAdventure(adventuresPool);
                while (randomAdventures.Find(adventure => adventure.Region == randomAdventure.Region) != null)
                {
                    randomAdventure = GetRandomAdventure(adventuresPool);
                }

                randomAdventures.Add(randomAdventure);
            }
            return randomAdventures;
        }

        public async Task GoOnAdventure(ComponentInteractionCreateEventArgs args, TimeGatedEvent timeGatedEvent, TimeGatedEvent previousEvent)
        {
            if (timeGatedEvent == null) return;

            CurrentUser.Stats.TimesTraveled++;
            TimeGatedAction.TickEvent(CurrentUser, cachedHandler, timeGatedEvent, previousEvent);
            await Save();

            await args.Interaction.CreateResponseAsync(
                InteractionResponseType.UpdateMessage,
                new DiscordInteractionResponseBuilder().AddEmbed(TimeGatedAction.GetEventEmbed(CurrentUser, timeGatedEvent)));
        }

        private async Task Save()
        {
            var updateQuery = new UserUpdateQueryBuilder(CurrentUser, QueryElement.Wallet, QueryElement.Stats, QueryElement.Events, QueryElement.Characters).Build();
            await cachedHandler.SendData(CurrentUser, updateQuery);
        }*/
    }
}
