using Eremite.Actions;
using Eremite.Services;
using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;
using DSharpPlus.Entities;
using DSharpPlus;
using Eremite.Data.DiscordData;
using DSharpPlus.EventArgs;
using Eremite.Builders;

namespace Eremite.Commands
{
    public sealed class PullCommand : BaseCommandModule
    {
        public DataHandler DataHandler { get; set; }
        public PullAction PullAction { get; set; }

        private readonly string overviewKey = "pull.set_new_char_as_main";
        private readonly string setKey = "pull.overview_new_char_info";

        [Command("pull"), Description("Pull for a character X times")]
        public async Task PullCharacter(CommandContext context, int number)
        {
            var user = await DataHandler.GetData(context.User);

            if (user.Wallet.Primogems < DataHandler.Config.PullCost * number) await context.RespondAsync($"> {Localization.NoCurrencyKey}");
            else
            {
                var charactersPulled = await PullAction.ForUserAsyncSave(user, number);
                var buttons = CreateButtons(context, user, charactersPulled.GetHighestTier());

                await context.RespondAsync(new DiscordMessageBuilder()
                    .WithEmbed(PullAction.GetEmbedWithCharacters(charactersPulled, user))
                    .AddComponents(buttons.Keys));
            };
        }

        [Command("pull"), Description("Pull for a character onces")]
        public async Task PullCharacter(CommandContext context) => await PullCharacter(context, 1);

        private Dictionary<DiscordButtonComponent, string> CreateButtons(CommandContext context, UserData user, Character highestTier)
        {
            var setMainGuid = Guid.NewGuid().ToString();
            var statsGuid = Guid.NewGuid().ToString();

            var setMainButton = new DiscordButtonComponent(ButtonStyle.Primary, setMainGuid, Localization.GetText(setKey));
            var statsButton = new DiscordButtonComponent(ButtonStyle.Secondary, statsGuid, Localization.GetText(overviewKey));

            context.Client.ComponentInteractionCreated += async (client, args) =>
            {
                if (args.User.Id.ToString() != user.UserId) return;

                if (args.Id == setMainGuid) await SetMainCharacter(args, user, highestTier);
                if (args.Id == statsGuid) await ShowCharacterStats(args, highestTier);
            };

            return new Dictionary<DiscordButtonComponent, string>()
            {
                { setMainButton, setMainGuid },
                { statsButton, statsGuid }
            };
        }

        private async Task SetMainCharacter(ComponentInteractionCreateEventArgs args, UserData user, Character highestTier)
        {
            SetCharacterAction.Equip(user, highestTier);
            await DataHandler.SendData(user, new UserUpdateQueryBuilder(user, Data.QueryElement.EquippedCharacter).Build());
            await args.Interaction.CreateResponseAsync(
                    InteractionResponseType.UpdateMessage,
                    new DiscordInteractionResponseBuilder()
                    .AddEmbed(SetCharacterAction.GetEmbedWithCharacterInfo(highestTier)));
        }

        private async Task ShowCharacterStats(ComponentInteractionCreateEventArgs args, Character highestTier)
        {
            await args.Interaction.CreateResponseAsync(
                    InteractionResponseType.UpdateMessage,
                    new DiscordInteractionResponseBuilder()
                    .AddEmbed(SetCharacterAction.GetEmbedWithCharacterInfo(highestTier)));
        }
    }
}
