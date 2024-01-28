using DSharpPlus;
using DSharpPlus.Entities;
using DSharpPlus.EventArgs;
using Eremite.Actions;
using Eremite.Services;
using Eremite.Data.DiscordData;
using Eremite.Layouts;
using DSharpPlus.SlashCommands;

namespace Eremite.SlashCommands
{
    public sealed class AkashaCommand : ApplicationCommandModule
    {
        public DataHandler DataHandler { get; set; }
        public PullAction PullAction { get; set; }

        private readonly string pullKey = "profile.pull_button";
        private readonly string statsKey = "profile.account_stats_button";
        private readonly string overviewKey = "pull.overview_new_char_info";
        private readonly string setKey = "pull.set_new_char_as_main";

        private AkashaLayout _layout;

        [SlashCommand("akasha", "Shows the current user profile with the current equipped character, mora and primos")]
        public async Task ShowAkasha(InteractionContext context)
        {
            var user = await DataHandler.GetData(context.User);
            new InfoAction(DataHandler, context, user);
            
            _layout = new AkashaLayout(user, DataHandler.Config.DefaultAkashaImageUrl);

            var buttons = CreateButtons(user, context);

            var currentCharacter = CharactersHandler.ConvertId(user.EquippedCharacter);
            var characterIdsConverted = CharactersHandler.ConvertIds(user.Characters);

            var messageBuilder = new DiscordInteractionResponseBuilder()
                .AddComponents(buttons.Keys)
                .AddEmbed(_layout.GetMainAkashaEmbed(user, characterIdsConverted, currentCharacter));

            await context.CreateResponseAsync(messageBuilder);
        }

        [SlashCommand("profile", "Shows the current user profile with the current equipped character, mora and primos")]
        public async Task ShowProfile(InteractionContext context) => await ShowAkasha(context);

        public Dictionary<DiscordButtonComponent, string> CreateButtons(UserData user, InteractionContext context)
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

        private async Task Pull(InteractionContext context, ComponentInteractionCreateEventArgs args, UserData user)
        {
            if (user.Wallet.Primogems < DataHandler.Config.PullCost) await context.CreateResponseAsync(new DiscordInteractionResponseBuilder().WithContent($"> {user.GetText(Services.Localization.NoCurrencyKey)}"));
            else
            {
                var pullResult = await PullAction.ForUserAsyncSave(user, 1);
                var charactersPulled = pullResult.Item1;
                var cashback = pullResult.Item2;

                var highestTier = charactersPulled.GetHighestTier();

                var setCharacterGuid = Guid.NewGuid().ToString();
                var infoAboutCharacterGuid = Guid.NewGuid().ToString();

                await args.Interaction.CreateResponseAsync(InteractionResponseType.UpdateMessage,
                    new DiscordInteractionResponseBuilder().AddEmbed(PullAction.GetEmbedWithCharacters(charactersPulled, CashbackAction.GetCashbackMessage(user, cashback), user))
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
