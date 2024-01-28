using DSharpPlus.Entities;
using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;
using Eremite.Actions;
using Eremite.Services;
using Eremite.Data.DiscordData;
using Eremite.Data;
using Eremite.Builders;

namespace Eremite.Commands
{
    public sealed class ShopCommand : BaseCommandModule
    {
        public DataHandler DataHandler { get; set; }

        private readonly string inputPlaceholder = "shop.input_placeholder";

        [Command("shop"), Description("Shows the current shop lots and prices from Dori herself.")]
        public async Task ShowShop(CommandContext context)
        {
            var user = await DataHandler.GetData(context.User);
            new InfoAction(DataHandler, context, user);

            var shopAction = new ShopAction(user, DataHandler);

            var options = shopAction.CreateShopDropdown(context, user);
            var dropdown = new DiscordSelectComponent("shopdropdown", user.GetText(inputPlaceholder), options.Values, false, 1, 1);

            var messageBuilder = new DiscordMessageBuilder()
                .AddComponents(dropdown)
                .WithEmbed(ShopAction.GetEmbedWithShopInfo(user));

            shopAction.OnUserBought += SaveData;
            await context.RespondAsync(messageBuilder);
        }

        private async void SaveData(UserData user, DoriLot lot)
        {
            var query = new UserUpdateQueryBuilder(user, QueryElement.Events, QueryElement.Wallet, QueryElement.Stats, QueryElement.Characters);
            await DataHandler.SendData(user, query.Build());
        }
    }
}
