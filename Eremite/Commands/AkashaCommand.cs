using DSharpPlus;
using DSharpPlus.Entities;
using DSharpPlus.EventArgs;
using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;
using Eremite.Services;
using Eremite.Data.DiscordData;
using Eremite.Actions;

namespace Eremite.Commands
{
    public sealed class AkashaCommand : BaseCommandModule
    {
        public DataHandler DataHandler { get; set; }

        public const string StarSign = ":star:";
        public const string DefaultNullError = "None, use !pull to get one.";

        [Command("akasha"), Description("Shows the current user profile with the current equipped character, mora and primos")]
        public async Task ShowProfile(CommandContext context)
        {
            var buttons = CreateButtons(context);
            var user = await DataHandler.GetData(context.User);

            var currentCharacter = user.EquippedCharacter;

            string currentCharacterName = currentCharacter == null ? DefaultNullError : currentCharacter.CharacterName;
            string charactersInInventory = GetCharactersFromInventory(user);
            string profileImageUrl = user.EquippedCharacter == null ? DataHandler.Config.DefaultAkashaImageUrl : user.EquippedCharacter.ImageAkashaBannerUrl;

            string characterBuffInfo = user.EquippedCharacter == null ? "None, use !setcharacter [name] or !pull to get one :)" : user.EquippedCharacter.PerkInfo;

            var messageBuilder = new DiscordMessageBuilder()
                .AddComponents(buttons.Keys)
                .WithEmbed(new DiscordEmbedBuilder()
                {
                    Color = DiscordColor.Orange,
                    Title = $"{user.Username}'s profile",
                    ImageUrl = profileImageUrl,
                    Description = $"[ID:{user.UserId}]\n\n> **Main Character: {currentCharacterName}**" +
                    $"\n> Character Buff: {characterBuffInfo}\n\nCharacters Obtained: {charactersInInventory}" +
                    $"\n\n`Primogems: {user.Wallet.Primogems} | Mora: {user.Wallet.Mora}`"
                });

            await context.RespondAsync(messageBuilder);
        }

        private Dictionary<DiscordButtonComponent, string> CreateButtons(CommandContext context)
        {
            var pullGuid = Guid.NewGuid().ToString();
            var shopGuid = Guid.NewGuid().ToString();
            var statsGuid = Guid.NewGuid().ToString();

            var pullButton = new DiscordButtonComponent(ButtonStyle.Success, pullGuid, "Pull");
            var shopButton = new DiscordButtonComponent(ButtonStyle.Secondary, shopGuid, "Mora Shop");
            var statsButton = new DiscordButtonComponent(ButtonStyle.Secondary, statsGuid, "Account Stats");

            context.Client.ComponentInteractionCreated += async (client, args) =>
            {
                if (args.User.Id.ToString() != context.User.Id.ToString()) return;

                if (args.Id == statsGuid) await ShowStats(context, args);
            };

            return new Dictionary<DiscordButtonComponent, string>()
            {
                { pullButton, pullGuid },
                { shopButton, shopGuid },
                { statsButton, statsGuid }
            };
        }

        private async Task ShowStats(CommandContext context, ComponentInteractionCreateEventArgs args)
        {
            var user = await DataHandler.GetData(context.User);
            var embed = StatsAction.GetEmbedWithStats(context.User.AvatarUrl, user);

            await args.Interaction.CreateResponseAsync(InteractionResponseType.UpdateMessage,
                    new DiscordInteractionResponseBuilder().AddEmbed(embed));
        }

        private string GetCharactersFromInventory(UserData user)
        {
            if (user.Characters.Count <= 0) return DefaultNullError;

            string charactersInInventory = string.Empty;
            foreach (var character in user.Characters)
            {
                charactersInInventory = $"{charactersInInventory} {character.CharacterName}<{character.StarsRarity}{StarSign}> ";
            }

            return charactersInInventory;
        }
    }
}
