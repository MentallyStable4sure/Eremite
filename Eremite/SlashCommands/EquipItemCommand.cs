using Eremite.Actions;
using Eremite.Services;
using DSharpPlus.SlashCommands;
using Eremite.Data;
using Eremite.Builders;
using Newtonsoft.Json;
using Eremite.Data.DiscordData;

namespace Eremite.SlashCommands
{
    public sealed class EquipItemCommand : ApplicationCommandModule
    {
        public DataHandler DataHandler { get; set; }

        public const string equippedItemKey = "profile.current_item";

        [SlashCommand("equip", "Shows the current user profile with the current equipped character, mora and primos")]
        public async Task EquipItem(InteractionContext context, [Option("itemId", "Item to equip")] long itemId)
        {
            var user = await DataHandler.GetData(context.User);
            new InfoAction(DataHandler, context, user);

            if (ItemsDb.Items.ContainsKey((int)itemId)) user.Stats.EquippedItem = ItemsDb.Items[(int)itemId];
            if (ItemsDb.Fishes.ContainsKey((int)itemId)) user.Stats.EquippedItem = ItemsDb.Fishes[(int)itemId];
            if (ItemsDb.FishingRods.ContainsKey((int)itemId)) user.Stats.EquippedItem = ItemsDb.FishingRods[(int)itemId];

            if (user.Stats.EquippedItem == null) return;
            if (user.Stats.EquippedItem.ItemId != itemId) return;

            var updateQuery = new UserUpdateQueryBuilder(user, QueryElement.Stats, QueryElement.Inventory).Build();
            await DataHandler.SendData(user, updateQuery);
            await context.CreateResponseAsync($"> {user.GetText(equippedItemKey)} {user.Stats.EquippedItem.EmojiCode}");
        }
    }
}
