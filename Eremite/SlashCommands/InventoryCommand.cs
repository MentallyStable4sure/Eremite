﻿using Eremite.Actions;
using Eremite.Services;
using DSharpPlus.SlashCommands;
using System.Text;

namespace Eremite.SlashCommands
{
    public sealed class InventoryCommand : ApplicationCommandModule
    {
        public DataHandler DataHandler { get; set; }

        [SlashCommand("inventory", "Shows all owned inventory items")]
        public async Task InventoryList(InteractionContext context)
        {
            var user = await DataHandler.GetData(context.User);
            new InfoAction(DataHandler, context, user);

            var sb = new StringBuilder();

            foreach (var item in user.Inventory)
            {
                var price = item.SellPrice;
                sb.AppendLine($"> {item.ItemId} | {item.EmojiCode} | x{item.Amount} | SELL PRICE: {price.Mora}{Services.Localization.MoraEmoji} {price.Primogems}{Services.Localization.PrimosEmoji} {price.Pills}{Services.Localization.PillsEmoji}");
            }

            await context.CreateResponseAsync(sb.ToString());
        }
    }
}
