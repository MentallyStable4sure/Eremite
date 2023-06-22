using DSharpPlus.Entities;
using DSharpPlus.EventArgs;
using DSharpPlus;
using Eremite.Data.DiscordData;
using Eremite.Data;
using Eremite.Services;
using Eremite.Builders;

namespace Eremite.Actions
{
    public class AdventureAction
    {
        public const TimeGatedEventType AdventuresType = TimeGatedEventType.Adventure;
        public const int ChoicesPerAdventure = 2;

        public static AdventureEvent GetRandomAdventure(List<AdventureEvent> adventures) => adventures[Random.Shared.Next(0, adventures.Count)];

        public static List<AdventureEvent> FillRandomAdventures(List<AdventureEvent> adventuresPool, int amount = ChoicesPerAdventure)
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

        public static async Task GoOnAdventure(DataHandler dataHandler, ComponentInteractionCreateEventArgs args, UserData user, TimeGatedEvent timeGatedEvent, TimeGatedEvent previousEvent)
        {
            if (timeGatedEvent == null) return;

            user.Stats.TimesTraveled++;
            TimeGatedAction.TickEvent(user, timeGatedEvent, previousEvent);
            await Save(dataHandler, user);

            await args.Interaction.CreateResponseAsync(
                InteractionResponseType.UpdateMessage,
                new DiscordInteractionResponseBuilder().AddEmbed(TimeGatedAction.GetEventEmbed(user, timeGatedEvent)));
        }

        private static async Task Save(DataHandler dataHandler, UserData user)
        {
            var updateQuery = new UserUpdateQueryBuilder(user, QueryElement.Wallet, QueryElement.Stats, QueryElement.Events, QueryElement.Characters).Build();
            await dataHandler.SendData(user, updateQuery);
        }
    }
}
