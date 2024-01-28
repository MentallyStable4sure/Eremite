using Eremite.Actions;
using Eremite.Services;
using DSharpPlus.Entities;
using DSharpPlus;
using Eremite.Data.DiscordData;
using DSharpPlus.EventArgs;
using Eremite.Builders;
using DSharpPlus.SlashCommands;

namespace Eremite.SlashCommands
{
    public sealed class PullCommand : ApplicationCommandModule
    {
        public DataHandler DataHandler { get; set; }
        public PullAction PullAction { get; set; }

        private readonly string overviewKey = "pull.overview_new_char_info";
        private readonly string setKey = "pull.set_new_char_as_main";

        [SlashCommand("pull", "Pull for a character X times")]
        public async Task PullCharacter(InteractionContext context, [Option("number", "Number of times to pull at once")] long number)
        {
            if (number > 50) return;

            var user = await DataHandler.GetData(context.User);
            new InfoAction(DataHandler, context, user);
            var message = new DiscordFollowupMessageBuilder();

            if (user.Wallet.Primogems < DataHandler.Config.PullCost * number) await context.FollowUpAsync(message.WithContent($"> {user.GetText(Services.Localization.NoCurrencyKey)}"));
            else
            {
                var pullResult = await PullAction.ForUserAsyncSave(user, (int)number);
                var charactersPulled = pullResult.Item1;
                var cashback = pullResult.Item2;

                var buttons = CreateButtons(context, user, charactersPulled.GetHighestTier());

                await context.FollowUpAsync(message
                    .AddEmbed(PullAction.GetEmbedWithCharacters(charactersPulled, CashbackAction.GetCashbackMessage(user, cashback), user))
                    .AddComponents(buttons.Keys));
            };
        }

        [SlashCommand("pull", "Pull for a character onces")]
        public async Task PullCharacter(InteractionContext context) => await PullCharacter(context, 1);

        private Dictionary<DiscordButtonComponent, string> CreateButtons(InteractionContext context, UserData user, Character highestTier)
        {
            var setMainGuid = Guid.NewGuid().ToString();
            var statsGuid = Guid.NewGuid().ToString();

            var setMainButton = new DiscordButtonComponent(ButtonStyle.Primary, setMainGuid, user.GetText(setKey));
            var statsButton = new DiscordButtonComponent(ButtonStyle.Secondary, statsGuid, user.GetText(overviewKey));

            context.Client.ComponentInteractionCreated += async (client, args) =>
            {
                if (args.User.Id.ToString() != user.UserId) return;

                if (args.Id == setMainGuid) await SetMainCharacter(args, user, highestTier);
                if (args.Id == statsGuid) await ShowCharacterStats(args, user, highestTier);
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
                    .AddEmbed(SetCharacterAction.GetEmbedWithCharacterInfo(user, highestTier)));
        }

        private async Task ShowCharacterStats(ComponentInteractionCreateEventArgs args, UserData user, Character highestTier)
        {
            await args.Interaction.CreateResponseAsync(
                    InteractionResponseType.UpdateMessage,
                    new DiscordInteractionResponseBuilder()
                    .AddEmbed(SetCharacterAction.GetEmbedWithCharacterInfo(user, highestTier)));
        }
    }
}
