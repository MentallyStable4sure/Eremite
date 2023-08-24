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

        private readonly string pullKey = "profile.pull_button";
        private readonly string statsKey = "profile.account_stats_button";
        private readonly string overviewKey = "pull.overview_new_char_info";
        private readonly string setKey = "pull.set_new_char_as_main";

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
                .WithEmbed(_layout.GetMainAkashaEmbed(user, characterIdsConverted, currentCharacter));

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
                { new DiscordButtonComponent(ButtonStyle.Success, pullGuid, user.GetText(pullKey)), pullGuid },
                { new DiscordButtonComponent(ButtonStyle.Secondary, statsGuid, user.GetText(statsKey)), statsGuid }
            };
        }

        private async Task Pull(CommandContext context, ComponentInteractionCreateEventArgs args, UserData user)
        {
            if (user.Wallet.Primogems < DataHandler.Config.PullCost) await context.RespondAsync($"> {user.GetText(Localization.NoCurrencyKey)}");
            else
            {
                var charactersPulled = await PullAction.ForUserAsyncSave(user, 1);
                var highestTier = charactersPulled.GetHighestTier();

                var setCharacterGuid = Guid.NewGuid().ToString();
                var infoAboutCharacterGuid = Guid.NewGuid().ToString();

                await args.Interaction.CreateResponseAsync(InteractionResponseType.UpdateMessage,
                    new DiscordInteractionResponseBuilder().AddEmbed(PullAction.GetEmbedWithCharacters(charactersPulled, user))
                    .AddComponents(
                        new DiscordButtonComponent(ButtonStyle.Primary, setCharacterGuid, user.GetText(setKey)),
                        new DiscordButtonComponent(ButtonStyle.Secondary, infoAboutCharacterGuid, user.GetText(overviewKey))));

                context.Client.ComponentInteractionCreated += async (client, args) =>
                {
                    if (args.User.Id.ToString() != context.User.Id.ToString()) return;

                    if (args.Id == setCharacterGuid) await AkashaAction.EquipCharacter(args, highestTier, DataHandler, user);
                    if (args.Id == infoAboutCharacterGuid) await AkashaAction.ShowCharacterStats(user, args, highestTier);
                };
            };
        }
    }
}
