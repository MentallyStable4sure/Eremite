using DSharpPlus;
using DSharpPlus.Entities;
using DSharpPlus.EventArgs;
using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;
using Eremite.Actions;
using Eremite.Services;
using Eremite.Data.DiscordData;

namespace Eremite.Commands
{
    public sealed class AkashaCommand : BaseCommandModule
    {
        public DataHandler DataHandler { get; set; }
        public PullAction PullAction { get; set; }

        public const string StarSign = "✦";
        public const string DefaultNullError = "None, use !pull to get one.";

        [Command("akasha"), Description("Shows the current user profile with the current equipped character, mora and primos")]
        public async Task ShowAkasha(CommandContext context)
        {
            var user = await DataHandler.GetData(context.User);
            var buttons = CreateButtons(context, user);

            var currentCharacter = user.EquippedCharacter;
            var sacrificePrice = currentCharacter == null ? 0 : currentCharacter.SellPrice;

            string currentCharacterName = currentCharacter == null ? DefaultNullError : currentCharacter.CharacterName;
            string profileImageUrl = currentCharacter == null ? DataHandler.Config.DefaultAkashaImageUrl : user.EquippedCharacter.ImageAkashaBannerUrl;

            string characterBuffInfo = user.EquippedCharacter == null ? "None, use !setcharacter [name] or !pull to get one :)" : user.EquippedCharacter.PerkInfo;

            var messageBuilder = new DiscordMessageBuilder()
                .AddComponents(buttons.Keys)
                .WithEmbed(new DiscordEmbedBuilder()
                {
                    Color = DiscordColor.Orange,
                    Title = $"{user.Username}'s profile",
                    ImageUrl = profileImageUrl,
                    Description = $"[ID:{user.UserId}]\n\n> **Main Character: {currentCharacterName}**" +
                    $"\n> Character Buff: {characterBuffInfo}\n\nCharacters Obtained: {user.Characters.ToCharacterList()}" +
                    $"\n\n`Primogems: {user.Wallet.Primogems} | Mora: {user.Wallet.Mora} | {user.Wallet.Pills} 💊`"
                });

            await context.RespondAsync(messageBuilder);
        }

        [Command("profile"), Description("Shows the current user profile with the current equipped character, mora and primos")]
        public async Task ShowProfile(CommandContext context) => await ShowAkasha(context);

        private Dictionary<DiscordButtonComponent, string> CreateButtons(CommandContext context, UserData user)
        {
            var pullGuid = Guid.NewGuid().ToString();
            var shopGuid = Guid.NewGuid().ToString();
            var statsGuid = Guid.NewGuid().ToString();

            context.Client.ComponentInteractionCreated += async (client, args) =>
            {
                if (args.User.Id.ToString() != context.User.Id.ToString()) return;

                if (args.Id == statsGuid) await AkashaAction.ShowAccountStats(context, args, user);
                if (args.Id == pullGuid) await Pull(context, args, user);
                if (args.Id == shopGuid) await AkashaAction.ShowShop(context, args, user, DataHandler);
            };

            return new Dictionary<DiscordButtonComponent, string>()
            {
                { new DiscordButtonComponent(ButtonStyle.Success, pullGuid, "Pull"), pullGuid },
                { new DiscordButtonComponent(ButtonStyle.Secondary, shopGuid, "Mora Shop"), shopGuid },
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

                    if (args.Id == setCharacterGuid) await EquipCharacter(args, user, highestTier);
                    if (args.Id == infoAboutCharacterGuid) await AkashaAction.ShowCharacterStats(args, highestTier);
                };
            };
        }

        private async Task EquipCharacter(ComponentInteractionCreateEventArgs args, UserData user, Character highestTier)
        {
            SetCharacterAction.Equip(user, highestTier);
            await DataHandler.SendData(user, new QueryBuilder(user, Data.QueryElement.EquippedCharacter).BuildUpdateQuery());
            await args.Interaction.CreateResponseAsync(InteractionResponseType.UpdateMessage,
                new DiscordInteractionResponseBuilder().AddEmbed(SetCharacterAction.GetEmbedWithCharacterInfo(highestTier)));
        }
    }
}
