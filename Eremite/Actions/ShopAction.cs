using DSharpPlus;
using DSharpPlus.Entities;
using DSharpPlus.EventArgs;
using Eremite.Data.DiscordData;
using Eremite.Services;

namespace Eremite.Actions
{
    public class ShopAction
    {
        public const string DoriShopBannerUrl = "https://raw.githubusercontent.com/MentallyStable4sure/Eremite/main/content/events/Shop.jpg";

        public const string OneHundredPrimosSlotId = "3000mora_to_100primos";
        public const string SignoraSlotId = "400pills_into_signora";
        public const string WelkinMoonSlotId = "2000pills_into_welkinmoon";
        public const string OneHundredPillsSlotId = "10000morainto100pills";

        public const string NotEnoughMaterialsError = "> Not enough materials to buy this item. Try using !adventure, !pull, !daily, etc.";

        public static DiscordEmbedBuilder GetEmbedWithShopInfo()
        {
            return new DiscordEmbedBuilder()
            {
                Color = DiscordColor.Purple,
                Title = "Dori's ~~jaw droppin prices~~ brilliant shop!",
                ImageUrl = DoriShopBannerUrl,
                Description = $"> For explosive discounts, go to http://mentallystable4sure.dev and visit the 'Alcazarzaray Palace' which is exclusively avaliable in MS Launcher."
            };
        }

        public static DiscordSelectComponent CreateShopDropdown()
        {
            var options = new List<DiscordSelectComponentOption>()
            {
                new DiscordSelectComponentOption(
                    "3000 Mora --> 100 Primogems",
                    OneHundredPrimosSlotId,
                    "Thats a great deal, trust Dori!",
                    emoji: new DiscordComponentEmoji(1113103136991756328)),

                new DiscordSelectComponentOption(
                    "400 Pills --> Crimson Witch Hat",
                    SignoraSlotId,
                    "Will make anyone a pyro witch!",
                    emoji: new DiscordComponentEmoji(1119701575313653924)),

                new DiscordSelectComponentOption(
                    "2000 Pills --> Welkin Moon",
                    WelkinMoonSlotId,
                    "It will cost u only 1000 if u trade it in our launcher",
                    emoji: new DiscordComponentEmoji(765128125301915658)),

                new DiscordSelectComponentOption(
                    "10000 Mora --> 100 Pills",
                    OneHundredPillsSlotId,
                    "I love mora! And Mora loves me! Hehe~",
                    emoji : new DiscordComponentEmoji(1119700330259693629))
            };

            return new DiscordSelectComponent("dropdown", "Dori will get your item from market", options, false, 1, 4);
        }

        internal static string Buy(UserData user, DoriLot lot, Action onSuccess = null)
        {
            var allCharacters = CharactersHandler.CharactersData;
            switch (lot)
            {
                case DoriLot.ONE_HUNDRED_PRIMOS:
                    if (user.Wallet.Mora < 3000) return NotEnoughMaterialsError;
                    user.Wallet.Mora -= 3000;
                    user.Wallet.Primogems += 100;
                    return string.Empty;
                case DoriLot.CRIMSON_WITCH_HAT:
                    if (user.Wallet.Pills < 400) return NotEnoughMaterialsError;
                    user.Wallet.Pills -= 400;
                    user.AddPulledCharacter(allCharacters.Find(character => character.CharacterName.ToLower().Contains("signora")).CharacterId);
                    break;
                case DoriLot.WELKIN_MOON:
                    if (user.Wallet.Pills < 2000) return NotEnoughMaterialsError;
                    return "Welkin Moon lot currently unavaliable... Sorry.";
                    user.Wallet.Pills -= 2000;
                    //TODO: Connect PayPal or better redirect on ms4s/php
                    break;
                case DoriLot.ONE_HUNDRED_PILLS:
                    if (user.Wallet.Mora < 10000) return NotEnoughMaterialsError;
                    user.Wallet.Mora -= 10000;
                    user.Wallet.Pills += 100;
                    break;
            }

            onSuccess?.Invoke();
            return string.Empty;
        }

        public static async Task ShowFeedbackFromShop(string response, ComponentInteractionCreateEventArgs args)
        {
            string message = string.Empty;

            if (response == null || response == string.Empty) message = "> Successfully bought item(s).";
            else message = response;

            await args.Interaction.CreateResponseAsync(InteractionResponseType.UpdateMessage, new DiscordInteractionResponseBuilder().WithContent(message));
        }

        public static async Task ShopInteracted(UserData user, ComponentInteractionCreateEventArgs args, Action OnSuccess = null)
        {
            if (args.Values.Contains(OneHundredPillsSlotId))
            {
                string response = Buy(user, DoriLot.ONE_HUNDRED_PILLS, OnSuccess);
                await ShowFeedbackFromShop(response, args);
            }

            if (args.Values.Contains(OneHundredPrimosSlotId))
            {
                string response = Buy(user, DoriLot.ONE_HUNDRED_PRIMOS, OnSuccess);
                await ShowFeedbackFromShop(response, args);
            }

            if (args.Values.Contains(SignoraSlotId))
            {
                string response = Buy(user, DoriLot.CRIMSON_WITCH_HAT, OnSuccess);
                await ShowFeedbackFromShop(response, args);
            }

            if (args.Values.Contains(WelkinMoonSlotId))
            {
                string response = Buy(user, DoriLot.WELKIN_MOON, OnSuccess);
                await ShowFeedbackFromShop(response, args);
            }
        }
    }
}
