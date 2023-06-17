using DSharpPlus.CommandsNext;
using DSharpPlus.Entities;
using DSharpPlus;
using Eremite.Data.DiscordData;
using DSharpPlus.EventArgs;
using Eremite.Services;
using Eremite.Data;

namespace Eremite.Actions
{
    public class AkashaAction
    {

        public static async Task ShowShop(CommandContext context, ComponentInteractionCreateEventArgs args, UserData user, DataHandler dataHandler)
        {
            var dropdown = ShopAction.CreateShopDropdown();
            var messageBuilder = new DiscordMessageBuilder()
                .AddComponents(dropdown)
                .WithEmbed(ShopAction.GetEmbedWithShopInfo());

            context.Client.ComponentInteractionCreated += async (sender, args) => await ShopAction.ShopInteracted(user, args, () => SaveDataFromShop(dataHandler, user));

            await args.Interaction.CreateResponseAsync(InteractionResponseType.UpdateMessage,
                new DiscordInteractionResponseBuilder(messageBuilder));
        }

        public static async Task ShowAccountStats(CommandContext context, ComponentInteractionCreateEventArgs args, UserData user)
        {
            var embed = StatsAction.GetEmbedWithStats(context.User.AvatarUrl, user);

            await args.Interaction.CreateResponseAsync(InteractionResponseType.UpdateMessage,
                    new DiscordInteractionResponseBuilder().AddEmbed(embed));
        }

        public static async Task ShowCharacterStats(ComponentInteractionCreateEventArgs args, Character character)
        {
            await args.Interaction.CreateResponseAsync(
                    InteractionResponseType.UpdateMessage,
                    new DiscordInteractionResponseBuilder().AddEmbed(SetCharacterAction.GetEmbedWithCharacterInfo(character)));
        }

        private async Task SaveDataFromShop(DataHandler dataHandler, UserData user)
        {
            var query = new QueryBuilder(user, QueryElement.Wallet, QueryElement.Stats, QueryElement.Characters);
            await dataHandler.SendData(user, query.BuildUpdateQuery());
        }
    }
}
