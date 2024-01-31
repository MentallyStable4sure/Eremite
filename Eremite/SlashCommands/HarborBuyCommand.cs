using Eremite.Actions;
using Eremite.Data;
using Eremite.Services;
using DSharpPlus.Entities;
using DSharpPlus.SlashCommands;

namespace Eremite.SlashCommands
{
    public sealed class HarborBuyCommand : ApplicationCommandModule
    {
        public DataHandler DataHandler { get; set; }

        [SlashCommand("harborbuy", "Buys an item from Liyue Harbor current market")]
        public async Task BuyItem(InteractionContext context, [Option("itemId", "Id of an item from /harbor")] long itemId)
        {
            var user = await DataHandler.GetData(context.User);
            new InfoAction(DataHandler, context, user);

            var message = new DiscordInteractionResponseBuilder();

            var harbor = new HarborAction(user, DataHandler);
            var response = await harbor.Buy(user, DataHandler, ItemsDb.GetItemById((int)itemId));

            if (response == string.Empty) response = $"✨ `{user.GetText(ShopAction.lotBought)}` ✅";
            await context.CreateResponseAsync(message.WithContent(response));
        }
    }
}
