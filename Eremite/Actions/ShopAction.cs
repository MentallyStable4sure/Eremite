using DSharpPlus;
using DSharpPlus.Entities;
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
        private DataHandler data;

        public Action<UserData, DoriLot> OnUserBought;

        public ShopAction(UserData user, DataHandler data)
        {
            _user = user;
            this.data = data;
        }

        public const string shopWelcome = "shop.welcome";
        public const string shopDescription = "shop.description";
        public const string lotUnavaliable = "shop.lot_unavaliable";
        public const string lotBought = "shop.lot_bought";
        public const string uidNeeded = "shop.lot_uid_needed";

        public const string witchHatKey = "shop.crimson_witch_hat";
        public const string welkinKey = "shop.welkin_moon";

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

        public Dictionary<Identifier, DiscordSelectComponentOption> CreateShopDropdown(DSharpPlus.SlashCommands.InteractionContext context, UserData user)
        {
            var oneHundredPrimosGuid = Guid.NewGuid().ToString();
            var crimsonWitchHatGuid = Guid.NewGuid().ToString();
            var welkinMoonGuid = Guid.NewGuid().ToString();
            var oneHundredPillsGuid = Guid.NewGuid().ToString();

            var dropdrownOptions = new Dictionary<Identifier, DiscordSelectComponentOption>()
            {
                { new Identifier(oneHundredPrimosGuid, (int)DoriLot.ONE_HUNDRED_PRIMOS), new DiscordSelectComponentOption(
                    $"✦ 3000 {user.GetText(Localization.Mora)} ✦ ➜➜ ✦ 100 {user.GetText(Localization.Primos)} ✦",
                    oneHundredPrimosGuid,
                    user.GetText(lot100Primogems),
                    emoji: new DiscordComponentEmoji(1113103136991756328)) },
                { new Identifier(crimsonWitchHatGuid, (int)DoriLot.CRIMSON_WITCH_HAT), new DiscordSelectComponentOption(
                    $"✦ 2500 {user.GetText(Localization.Pills)} ✦ ➜➜ ✦ {user.GetText(witchHatKey)} ✦",
                    crimsonWitchHatGuid,
                    user.GetText(lotCrimsonWitch),
                    emoji: new DiscordComponentEmoji(1119701575313653924)) },
                { new Identifier(welkinMoonGuid, (int)DoriLot.WELKIN_MOON), new DiscordSelectComponentOption(
                    $"✦ 5000 {user.GetText(Localization.Pills)} ✦ ➜➜ ✦ {user.GetText(welkinKey)} ✦",
                    welkinMoonGuid,
                    user.GetText(lotWelkin),
                    emoji: new DiscordComponentEmoji(765128125301915658)) },
                { new Identifier(oneHundredPillsGuid, (int)DoriLot.ONE_HUNDRED_PILLS), new DiscordSelectComponentOption(
                    $"✦ 10000 {user.GetText(Localization.Mora)} ✦ ➜➜ ✦ 300 {user.GetText(Localization.Pills)} ✦",
                    oneHundredPillsGuid,
                    user.GetText(lot100pills),
                    emoji : new DiscordComponentEmoji(1119700330259693629)) },
            };

            context.Client.ComponentInteractionCreated += async (client, args) =>
            {
                if (args.User.Id.ToString() != _user.UserId) return;

                foreach (var option in dropdrownOptions)
                {
                    if (args == null) continue;
                    if (!args.Values.Contains(option.Key.identifier)) continue;
                    string response = await Buy(_user, (DoriLot)option.Key.content);
                    string message = $"> {user.GetText(lotBought)}.";

                    if (response != null && response != string.Empty) message = response;
                    if (args.Interaction == null) continue;

                    await args.Interaction.CreateResponseAsync(InteractionResponseType.UpdateMessage, new DiscordInteractionResponseBuilder().WithContent(message));
                    return;
                }

                return;
            };


            return dropdrownOptions;
        }

        internal async Task<string> Buy(UserData user, DoriLot lot)
        {
            var allCharacters = CharactersHandler.CharactersData;
            switch (lot)
            {
                case DoriLot.ONE_HUNDRED_PRIMOS:
                    if (user.Wallet.Mora < 3000) return user.GetText(Localization.NoCurrencyKey);
                    user.Wallet.Mora -= 3000;
                    user.Wallet.Primogems += 100;
                    ChangeStats(user, new DiscordWallet(100, -3000));
                    break;

                case DoriLot.CRIMSON_WITCH_HAT:
                    if (user.Wallet.Pills < 2500) return user.GetText(Localization.NoCurrencyKey);
                    user.Wallet.Pills -= 2500;
                    user.AddPulledCharacter(allCharacters.Find(character => character.CharacterName.ToLower().Contains("signora")).CharacterId);
                    ChangeStats(user, new DiscordWallet(0, 0, -2500));
                    break;

                case DoriLot.WELKIN_MOON:
                    if (user.Wallet.Pills < 5000) return user.GetText(Localization.NoCurrencyKey);
                    if(!ConnectAction.CheckGenshinUID(user.Stats.UserUID)) return user.GetText(uidNeeded);

                    var canTrigger = user.HandleEvent(data, new TimeGatedEvent(TimeGatedEventType.Welkin, new TimeSpan(30, 0, 0, 0)));
                    if(!canTrigger)
                    {
                        var previousEvent = user.GetPreviousEventByType(TimeGatedEventType.Welkin);
                        string countdown = previousEvent.LastTimeTriggered.Add(previousEvent.TimeBetweenTriggers).Subtract(DateTime.UtcNow).GetNormalTime();
                        return $"> {user.GetText(TimeGatedAction.eventAlreadyTriggered)}. {user.GetText(TimeGatedAction.triggerTimeSuggestion)} {countdown}";
                    }

                    var result = await SellerAction.BuyWelkin(user.Stats.UserUID);
                    if (!result) return user.GetText(lotUnavaliable);

                    user.Wallet.Pills -= 5000;
                    ChangeStats(user, new DiscordWallet(0, 0, -5000));
                    break;

                case DoriLot.ONE_HUNDRED_PILLS:
                    if (user.Wallet.Mora < 10000) return user.GetText(Localization.NoCurrencyKey);
                    user.Wallet.Mora -= 10000;
                    user.Wallet.Pills += 300;
                    ChangeStats(user, new DiscordWallet(300, -10000, 0));
                    break;
            }

            OnUserBought?.Invoke(user, lot);
            return string.Empty;
        }

        private void ChangeStats(UserData user, DiscordWallet wallet)
        {
            if (wallet.Pills > 0) user.Stats.TotalPillsEarned += wallet.Pills;
            else user.Stats.TotalPillsSpent += wallet.Pills * -1;

            if (wallet.Primogems > 0) user.Stats.TotalPrimogemsEarned += wallet.Primogems;
            else user.Stats.TotalPrimogemsSpent += wallet.Primogems * -1;
        }
    }
}
