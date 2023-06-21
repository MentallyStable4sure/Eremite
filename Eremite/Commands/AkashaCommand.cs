using DSharpPlus;
using DSharpPlus.Entities;
using DSharpPlus.EventArgs;
using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;
using Eremite.Actions;
using Eremite.Services;
using Eremite.Data.DiscordData;
using Eremite.Layouts;

namespace Eremite.Commands
{
    public sealed class AkashaCommand : BaseCommandModule
    {
        public DataHandler DataHandler { get; set; }
        public PullAction PullAction { get; set; }

        public const string StarSign = "✦";
        public const string DefaultNullError = "None, use !pull to get one.";

        private AkashaLayout _layout;

        [Command("akasha"), Description("Shows the current user profile with the current equipped character, mora and primos")]
        public async Task ShowAkasha(CommandContext context)
        {
            var user = await DataHandler.GetData(context.User);
            _layout = new AkashaLayout(user, DataHandler.Config.DefaultAkashaImageUrl);

            var buttons = CreateButtons(user, context);

            var currentCharacter = CharactersHandler.ConvertId(user.EquippedCharacter);
            var characterIdsConverted = CharactersHandler.ConvertIds(user.Characters);

            var messageBuilder = new DiscordMessageBuilder()
                .AddComponents(buttons.Keys)
                .WithEmbed(_layout.GetMainAkashaEmbed(characterIdsConverted, currentCharacter));

            await context.RespondAsync(messageBuilder);
        }

        [Command("profile"), Description("Shows the current user profile with the current equipped character, mora and primos")]
        public async Task ShowProfile(CommandContext context) => await ShowAkasha(context);

        public Dictionary<DiscordButtonComponent, string> CreateButtons(UserData user, CommandContext context)
        {
            var pullGuid = Guid.NewGuid().ToString();
            var statsGuid = Guid.NewGuid().ToString();

            context.Client.ComponentInteractionCreated += async (sender, args) =>
            {
                if (args.User.Id.ToString() != user.UserId) return;

                if (args.Id == pullGuid) await Pull(context, args, user);
                if (args.Id == statsGuid) await AkashaAction.ShowAccountStats(context, args, user);
            };

            return new Dictionary<DiscordButtonComponent, string>()
            {
                { new DiscordButtonComponent(ButtonStyle.Success, pullGuid, "Pull"), pullGuid },
                { new DiscordButtonComponent(ButtonStyle.Secondary, statsGuid, "Account Stats"), statsGuid }
            };
        }

        private async Task Pull(CommandContext context, ComponentInteractionCreateEventArgs args, UserData user)
        {
            if (user.Wallet.Primogems < DataHandler.Config.PullCost) await context.RespondAsync(PullAction.NotEnoughPrimosError);
            else
            {
                var charactersPulled = await PullAction.ForUserAsyncSave(user, 1);
                var highestTier = charactersPulled.GetHighestTier();

                var setCharacterGuid = Guid.NewGuid().ToString();
                var infoAboutCharacterGuid = Guid.NewGuid().ToString();

                await args.Interaction.CreateResponseAsync(InteractionResponseType.UpdateMessage,
                    new DiscordInteractionResponseBuilder().AddEmbed(PullAction.GetEmbedWithCharacters(charactersPulled, user))
                    .AddComponents(
                        new DiscordButtonComponent(ButtonStyle.Primary, setCharacterGuid, $"Set {highestTier.CharacterName} as Main"),
                        new DiscordButtonComponent(ButtonStyle.Secondary, infoAboutCharacterGuid, $"Overview {highestTier.CharacterName} info")));

                context.Client.ComponentInteractionCreated += async (client, args) =>
                {
                    if (args.User.Id.ToString() != context.User.Id.ToString()) return;

                    if (args.Id == setCharacterGuid) await AkashaAction.EquipCharacter(args, highestTier, DataHandler, user);
                    if (args.Id == infoAboutCharacterGuid) await AkashaAction.ShowCharacterStats(args, highestTier);
                };
            };
        }
    }
}
