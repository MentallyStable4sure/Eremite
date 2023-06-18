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

        public AkashaAction(UserData user)
        {
            _user = user;
        }

        public async Task ShowAccountStats(CommandContext context, ComponentInteractionCreateEventArgs args)
        {
            var embed = StatsAction.GetEmbedWithStats(context.User.AvatarUrl, _user);

            await args.Interaction.CreateResponseAsync(InteractionResponseType.UpdateMessage,
                    new DiscordInteractionResponseBuilder().AddEmbed(embed));
        }

        public async Task ShowCharacterStats(ComponentInteractionCreateEventArgs args, Character character)
        {
            await args.Interaction.CreateResponseAsync(
                    InteractionResponseType.UpdateMessage,
                    new DiscordInteractionResponseBuilder().AddEmbed(SetCharacterAction.GetEmbedWithCharacterInfo(character)));
        }

        public async Task EquipCharacter(ComponentInteractionCreateEventArgs args, Character highestTier, DataHandler dataHandler)
        {
            SetCharacterAction.Equip(_user, highestTier);
            await dataHandler.SendData(_user, new UserUpdateQueryBuilder(_user, QueryElement.EquippedCharacter).Build());
            await args.Interaction.CreateResponseAsync(InteractionResponseType.UpdateMessage,
                new DiscordInteractionResponseBuilder().AddEmbed(SetCharacterAction.GetEmbedWithCharacterInfo(highestTier)));
        }
    }
}
