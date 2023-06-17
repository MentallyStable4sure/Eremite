using DSharpPlus.Entities;
using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;
using Eremite.Actions;
using Eremite.Services;
using Eremite.Data.DiscordData;
using Eremite.Data;

namespace Eremite.Commands
{
    public sealed class ShopCommand : BaseCommandModule
    {
        public DataHandler DataHandler { get; set; }

        private UserData userWhoUsedShop;

        [Command("shop"), Description("Shows the current shop lots and prices from Dori herself.")]
        public async Task ShowShop(CommandContext context)
        {
            var dropdown = ShopAction.CreateShopDropdown();

            userWhoUsedShop = await DataHandler.GetData(context.User);

            var messageBuilder = new DiscordMessageBuilder()
                .AddComponents(dropdown)
                .WithEmbed(ShopAction.GetEmbedWithShopInfo());

            context.Client.ComponentInteractionCreated += async (sender, args) =>
            {
                if (userWhoUsedShop.UserId != args.User.Id.ToString()) return;

                await ShopAction.ShopInteracted(userWhoUsedShop, args, () => SaveData(userWhoUsedShop));
            };

            await context.RespondAsync(messageBuilder);
        }

        private async Task SaveData(UserData user)
        {
            var query = new QueryBuilder(user, QueryElement.Wallet, QueryElement.Stats, QueryElement.Characters);
            await DataHandler.SendData(user, query.BuildUpdateQuery());
        }
    }
}
