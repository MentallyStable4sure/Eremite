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
        public const TimeGatedEventType FishblastingType = TimeGatedEventType.Fishblasting;

        public UserData CurrentUser { get; private set; }

        private readonly DataHandler cachedHandler;

        public FishblastingAction(UserData currentUser, DataHandler cachedHandler)
        {
            CurrentUser = currentUser;
            this.cachedHandler = cachedHandler;
        }

        public FishblastingEvent GetRandomSpot(List<FishblastingEvent> events) => events[Random.Shared.Next(0, events.Count)];

        public List<FishblastingEvent> FillRandomSpots(List<FishblastingEvent> allFishSpots, int amount)
        {
            var randomSpots = new List<FishblastingEvent>();
            while (randomSpots.Count < amount)
            {
                var randomSpot = GetRandomSpot(allFishSpots);
                while (randomSpots.Contains(randomSpot))
                {
                    randomSpot = GetRandomSpot(allFishSpots);
                }

                randomSpots.Add(randomSpot);
            }
            return randomSpots;
        }

        public async Task GoOnFishing(ComponentInteractionCreateEventArgs args, FishblastingEvent timeGatedEvent, TimeGatedEvent previousEvent)
        {
            if (timeGatedEvent == null) return;

            var fishesToCatch = timeGatedEvent.FishesCanBeFound;
            fishesToCatch.Sort();
            int highestId = fishesToCatch[fishesToCatch.Count - 1];
            int catchedFish = fishesToCatch[0];

            if (CurrentUser.Stats.EquippedItem != null && ItemsDb.FishingRods.ContainsKey(CurrentUser.Stats.EquippedItem.ItemId))
            {
                var currentRod = CurrentUser.Stats.EquippedItem;
                int increasedChanceByRod = currentRod.ItemId * 10 >= 100 ? 100 : currentRod.ItemId;

                bool isLucky = Random.Shared.Next(0, 101) <= increasedChanceByRod;
                if (isLucky)
                {
                    fishesToCatch = AddMoreLevelsToCatch(highestId, fishesToCatch, currentRod);
                }

                catchedFish = fishesToCatch[Random.Shared.Next(0, fishesToCatch.Count + 1)];
            }
            else catchedFish = fishesToCatch[Random.Shared.Next(0, fishesToCatch.Count + 1)];

            var inventory = new InventoryAction(CurrentUser);
            inventory.AddItem(catchedFish);

            TimeGatedAction.TickEvent(CurrentUser, cachedHandler, timeGatedEvent, previousEvent);
            await Save();

            await args.Interaction.CreateResponseAsync(
                InteractionResponseType.UpdateMessage,
                new DiscordInteractionResponseBuilder()
                .AddEmbed(TimeGatedAction.GetEventEmbed(CurrentUser, timeGatedEvent))
                .WithContent($"> ✨ You caught: {ItemsDb.Fishes[catchedFish].EmojiCode} 🆕"));
        }

        private List<int> AddMoreLevelsToCatch(int highestId, List<int> fishesToCatch, UserItem fishingRod)
        {
            int amountToAdd = fishingRod.ItemId; //level basically (the higher = the more fish can be catched)
            while (amountToAdd > 0 || !ItemsDb.Fishes.ContainsKey(highestId + 1))
            {
                amountToAdd--;
                fishesToCatch.Add(ItemsDb.Fishes[highestId + 1].ItemId);

                fishesToCatch.Sort();
                highestId = fishesToCatch[fishesToCatch.Count - 1];
            }

            return fishesToCatch;
        }

        private async Task Save()
        {
            var updateQuery = new UserUpdateQueryBuilder(CurrentUser, QueryElement.Wallet, QueryElement.Stats, QueryElement.Events, QueryElement.Characters, QueryElement.Inventory).Build();
            await cachedHandler.SendData(CurrentUser, updateQuery);
        }
    }
}
