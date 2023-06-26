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
        public static async Task ShowAccountStats(CommandContext context, ComponentInteractionCreateEventArgs args, UserData user)
        {
            var embed = StatsAction.GetEmbedWithStats(context.User.AvatarUrl, user);

            await args.Interaction.CreateResponseAsync(InteractionResponseType.UpdateMessage,
                    new DiscordInteractionResponseBuilder().AddEmbed(embed));
        }

        public static async Task ShowCharacterStats(UserData user, ComponentInteractionCreateEventArgs args, Character character)
        {
            await args.Interaction.CreateResponseAsync(
                    InteractionResponseType.UpdateMessage,
                    new DiscordInteractionResponseBuilder().AddEmbed(SetCharacterAction.GetEmbedWithCharacterInfo(user, character)));
        }

        public static async Task EquipCharacter(ComponentInteractionCreateEventArgs args, Character highestTier, DataHandler dataHandler, UserData user)
        {
            SetCharacterAction.Equip(user, highestTier);
            await dataHandler.SendData(user, new UserUpdateQueryBuilder(user, QueryElement.EquippedCharacter).Build());
            await args.Interaction.CreateResponseAsync(InteractionResponseType.UpdateMessage,
                new DiscordInteractionResponseBuilder().AddEmbed(SetCharacterAction.GetEmbedWithCharacterInfo(user,highestTier)));
        }
    }
}
