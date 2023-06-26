using DSharpPlus;
using DSharpPlus.CommandsNext;
using DSharpPlus.Entities;
using DSharpPlus.EventArgs;
using Eremite.Base;
using Eremite.Data.DiscordData;
using Eremite.Services;

namespace Eremite.Actions
{
    public class ShopAction
    {
        public const string DoriShopBannerUrl = "https://raw.githubusercontent.com/MentallyStable4sure/Eremite/main/content/events/dori.jpg";

        public const string NotEnoughMaterialsError = "> Not enough materials to buy this item. Try using !adventure, !pull, !daily, etc.";

        private UserData _user;

        public Action<UserData, DoriLot> OnUserBought;
        public Action<ComponentInteractionCreateEventArgs> OnShopInteracted;

        public ShopAction(UserData user) => _user = user;

        public const string shopWelcome = "shop.welcome";
        public const string shopDescription = "shop.description";
        public const string lotUnavaliable = "shop.lot_unavaliable";
        public const string lotBought = "shop.lot_bought";

        public const string witchHatEmoji = "<:crimsonwitchhat:1119701575313653924>";
        public const string welkinEmoji = "<:Resin:765128125301915658>";

        public const string lot100Primogems = "shop.lot.100primogems";
        public const string lotCrimsonWitch = "shop.lot.crimson_witch_hat";
        public const string lotWelkin = "shop.lot.welkin_moon";
        public const string lot100pills = "shop.lot.100pills";

        public static DiscordEmbedBuilder GetEmbedWithShopInfo(UserData user)
        {
            return new DiscordEmbedBuilder()
            {
                Color = DiscordColor.Purple,
                Title = user.GetText(shopWelcome),
                ImageUrl = DoriShopBannerUrl,
                Description = $"> {user.GetText(shopDescription)}"
            };
        }

        public Dictionary<Identifier, DiscordSelectComponentOption> CreateShopDropdown(CommandContext context, UserData user)
        {
            var oneHundredPrimosGuid = Guid.NewGuid().ToString();
            var crimsonWitchHatGuid = Guid.NewGuid().ToString();
            var welkinMoonGuid = Guid.NewGuid().ToString();
            var oneHundredPillsGuid = Guid.NewGuid().ToString();

            var dropdrownOptions = new Dictionary<Identifier, DiscordSelectComponentOption>()
            {
                { new Identifier(oneHundredPrimosGuid, (int)DoriLot.ONE_HUNDRED_PRIMOS), new DiscordSelectComponentOption(
                    $"3000 {Localization.MoraEmoji} --> 100 {Localization.PrimosEmoji}",
                    oneHundredPrimosGuid,
                    user.GetText(lot100Primogems),
                    emoji: new DiscordComponentEmoji(1113103136991756328)) },
                { new Identifier(crimsonWitchHatGuid, (int)DoriLot.CRIMSON_WITCH_HAT), new DiscordSelectComponentOption(
                    $"400 {Localization.PillsEmoji} --> {witchHatEmoji}",
                    crimsonWitchHatGuid,
                    user.GetText(witchHatEmoji),
                    emoji: new DiscordComponentEmoji(1119701575313653924)) },
                { new Identifier(welkinMoonGuid, (int)DoriLot.WELKIN_MOON), new DiscordSelectComponentOption(
                    $"2000 {Localization.PillsEmoji} --> {welkinEmoji}",
                    welkinMoonGuid,
                    user.GetText(lotWelkin),
                    emoji: new DiscordComponentEmoji(765128125301915658)) },
                { new Identifier(oneHundredPillsGuid, (int)DoriLot.ONE_HUNDRED_PILLS), new DiscordSelectComponentOption(
                    $"10000 {Localization.MoraEmoji} --> 100 {Localization.PillsEmoji}",
                    oneHundredPillsGuid,
                    user.GetText(lot100pills),
                    emoji : new DiscordComponentEmoji(1119700330259693629)) },
            };

            context.Client.ComponentInteractionCreated += async (client, args) =>
            {
                if (args.User.Id.ToString() != _user.UserId) return;

                foreach (var option in dropdrownOptions)
                {
                    if (!args.Values.Contains(option.Key.identifier)) continue;
                    string response = Buy(_user, (DoriLot)option.Key.content);

                    await ShowFeedbackFromShop(user, response, args);
                }

                OnShopInteracted?.Invoke(args);
            };


            return dropdrownOptions;
        }

        internal string Buy(UserData user, DoriLot lot)
        {
            var allCharacters = CharactersHandler.CharactersData;
            switch (lot)
            {
                case DoriLot.ONE_HUNDRED_PRIMOS:
                    if (user.Wallet.Mora < 3000) return NotEnoughMaterialsError;
                    user.Wallet.Mora -= 3000;
                    user.Wallet.Primogems += 100;
                    break;
                case DoriLot.CRIMSON_WITCH_HAT:
                    if (user.Wallet.Pills < 400) return NotEnoughMaterialsError;
                    user.Wallet.Pills -= 400;
                    user.AddPulledCharacter(allCharacters.Find(character => character.CharacterName.ToLower().Contains("signora")).CharacterId);
                    break;
                case DoriLot.WELKIN_MOON:
                    if (user.Wallet.Pills < 2000) return NotEnoughMaterialsError;
                    return user.GetText(lotUnavaliable);
                    user.Wallet.Pills -= 2000;
                    //TODO: Connect PayPal or better redirect on ms4s/php
                    break;
                case DoriLot.ONE_HUNDRED_PILLS:
                    if (user.Wallet.Mora < 10000) return NotEnoughMaterialsError;
                    user.Wallet.Mora -= 10000;
                    user.Wallet.Pills += 100;
                    break;
            }

            OnUserBought?.Invoke(user, lot);
            return string.Empty;
        }

        public static async Task ShowFeedbackFromShop(UserData user, string response, ComponentInteractionCreateEventArgs args)
        {
            string message = string.Empty;

            if (response == null || response == string.Empty) message = $"> {user.GetText(lotBought)}.";
            else message = response;

            await args.Interaction.CreateResponseAsync(InteractionResponseType.UpdateMessage, new DiscordInteractionResponseBuilder().WithContent(message));
        }
    }
}
