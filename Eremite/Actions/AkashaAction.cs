using DSharpPlus.CommandsNext;
using DSharpPlus.Entities;
using DSharpPlus;
using Eremite.Data.DiscordData;
using DSharpPlus.EventArgs;
using Eremite.Services;
using Eremite.Data;
using Eremite.Builders;

namespace Eremite.Actions
{
    public class AkashaAction
    {
        private UserData _user;

        public AkashaAction(UserData user) => _user = user;

        public async Task ShowShop(CommandContext context, ComponentInteractionCreateEventArgs messageArgs, UserData user, DataHandler dataHandler)
        {
            var dropdown = ShopAction.CreateShopDropdown();
            var messageBuilder = new DiscordMessageBuilder()
                .AddComponents(dropdown)
                .WithEmbed(ShopAction.GetEmbedWithShopInfo());

            context.Client.ComponentInteractionCreated += async (sender, args) =>
            {
                if (args.Id != dropdown.CustomId) return;
                Console.WriteLine(dropdown.CustomId);
                if (user.UserId != args.User.Id.ToString()) return;
                await ShopAction.ShopInteracted(user, args, async () => await SaveDataFromShop(dataHandler, user));
            };

            await messageArgs.Interaction.CreateResponseAsync(InteractionResponseType.UpdateMessage,
                new DiscordInteractionResponseBuilder(messageBuilder));
        }

        public async Task ShowAccountStats(CommandContext context, ComponentInteractionCreateEventArgs args, UserData user)
        {
            var embed = StatsAction.GetEmbedWithStats(context.User.AvatarUrl, user);

            await args.Interaction.CreateResponseAsync(InteractionResponseType.UpdateMessage,
                    new DiscordInteractionResponseBuilder().AddEmbed(embed));
        }

        public async Task ShowCharacterStats(ComponentInteractionCreateEventArgs args, Character character)
        {
            await args.Interaction.CreateResponseAsync(
                    InteractionResponseType.UpdateMessage,
                    new DiscordInteractionResponseBuilder().AddEmbed(SetCharacterAction.GetEmbedWithCharacterInfo(character)));
        }

        private async Task SaveDataFromShop(DataHandler dataHandler, UserData user)
        {
            var query = new UserUpdateQueryBuilder(user, QueryElement.Wallet, QueryElement.Stats, QueryElement.Characters);
            await dataHandler.SendData(user, query.Build());
        }

        public async Task EquipCharacter(ComponentInteractionCreateEventArgs args, UserData user, Character highestTier, DataHandler dataHandler)
        {
            SetCharacterAction.Equip(user, highestTier);
            await dataHandler.SendData(user, new UserUpdateQueryBuilder(user, Data.QueryElement.EquippedCharacter).Build());
            await args.Interaction.CreateResponseAsync(InteractionResponseType.UpdateMessage,
                new DiscordInteractionResponseBuilder().AddEmbed(SetCharacterAction.GetEmbedWithCharacterInfo(highestTier)));
        }
    }
}
