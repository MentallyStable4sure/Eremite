using Eremite.Actions;
using Eremite.Data;
using Eremite.Services;
using DSharpPlus.Entities;
using DSharpPlus.SlashCommands;

namespace Eremite.SlashCommands
{
    public sealed class HarborSellCommand : ApplicationCommandModule
    {
        public DataHandler DataHandler { get; set; }

        [SlashCommand("harborsell", "Sells your item from inventory to the Liyue Harbor")]
        public async Task SellItem(InteractionContext context, [Option("itemId", "Id of an item from /inventory")] long itemId)
        {
            var user = await DataHandler.GetData(context.User);
            new InfoAction(DataHandler, context, user);

            var message = new DiscordInteractionResponseBuilder();

            var harbor = new HarborAction(user, DataHandler);
            var response = await harbor.Sell(user, DataHandler, ItemsDb.GetItemById((int)itemId));
            if (response == string.Empty) response = "✨ `SOLD!` ✅";
            await context.CreateResponseAsync(message.WithContent(response));
        }
    }
}
