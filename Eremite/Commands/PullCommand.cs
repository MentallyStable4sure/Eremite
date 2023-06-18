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

        private List<Character> charactersPulled = new List<Character>();

        [Command("pull"), Description("Pull for a character X times")]
        public async Task PullCharacter(CommandContext context, int number)
        {
            var user = await DataHandler.GetData(context.User);

            if (user.Wallet.Primogems < DataHandler.Config.PullCost * number) await context.RespondAsync(PullAction.NotEnoughPrimosError);
            else
            {
                charactersPulled = await PullAction.ForUserAsyncSave(user, number);
                var buttons = CreateButtons(context, user);

                await context.RespondAsync(new DiscordMessageBuilder()
                    .WithEmbed(PullAction.GetEmbedWithCharacters(charactersPulled, user))
                    .AddComponents(buttons.Keys));
            };
        }

        [Command("pull"), Description("Pull for a character onces")]
        public async Task PullCharacter(CommandContext context) => await PullCharacter(context, 1);

        private Dictionary<DiscordButtonComponent, string> CreateButtons(CommandContext context, UserData user)
        {
            var setMainGuid = Guid.NewGuid().ToString();
            var statsGuid = Guid.NewGuid().ToString();
            var highestTier = charactersPulled.GetHighestTier();

            var setMainButton = new DiscordButtonComponent(ButtonStyle.Primary, setMainGuid, $"Set {highestTier.CharacterName} as Main");
            var statsButton = new DiscordButtonComponent(ButtonStyle.Secondary, statsGuid, $"Overview {highestTier.CharacterName} info");

            context.Client.ComponentInteractionCreated += async (client, args) =>
            {
                if (args.User.Id.ToString() != context.User.Id.ToString()) return;

                if (args.Id == setMainGuid) await SetMainCharacter(args, user, highestTier);
                if (args.Id == statsGuid) await ShowCharacterStats(args);
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
                    .AddEmbed(SetCharacterAction.GetEmbedWithCharacterInfo(charactersPulled.GetHighestTier())));
        }

        private async Task ShowCharacterStats(ComponentInteractionCreateEventArgs args)
        {
            await args.Interaction.CreateResponseAsync(
                    InteractionResponseType.UpdateMessage,
                    new DiscordInteractionResponseBuilder()
                    .AddEmbed(SetCharacterAction.GetEmbedWithCharacterInfo(charactersPulled.GetHighestTier())));
        }
    }
}
