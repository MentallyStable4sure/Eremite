using DSharpPlus;
using DSharpPlus.Entities;
using Eremite.Base;
using Eremite.Builders;
using Eremite.Data;
using Eremite.Data.DiscordData;
using Eremite.Services;
using System.Collections.ObjectModel;
using System.Text;

namespace Eremite.Actions
{
    public class HarborAction
    {
        public const string HarborBannerUrl = "https://raw.githubusercontent.com/MentallyStable4sure/Eremite/main/content/events/harbor.gif";

        public const string NotEnoughMaterialsError = "> Not enough materials to buy this item. Try using !adventure, !pull, !daily, etc.";

        private UserData _user;
        private DataHandler data;

        public Action<UserData, HarborLot> OnUserBought;

        public HarborAction(UserData user, DataHandler data)
        {
            _user = user;
            this.data = data;
        }

        public const string shopWelcome = "harbor.welcome";
        public const string shopDescription = "harbor.description";
        public const string lotUnavaliable = "shop.lot_unavaliable";

        public static DiscordEmbedBuilder GetEmbedWithShopInfo(UserData user)
        {
            return new DiscordEmbedBuilder()
            {
                Color = DiscordColor.Orange,
                Title = user.GetText(shopWelcome),
                ImageUrl = HarborBannerUrl,
                Description = $"> {user.GetText(shopDescription)}"
            };
        }

        public Dictionary<Identifier, DiscordSelectComponentOption> CreateShopDropdown(DSharpPlus.SlashCommands.InteractionContext context, UserData user)
        {
            var fishingRoadGuid = Guid.NewGuid().ToString();
            var fishesGuid = Guid.NewGuid().ToString();
            var itemsGuid = Guid.NewGuid().ToString();

            var dropdrownOptions = new Dictionary<Identifier, DiscordSelectComponentOption>()
            {
                { new Identifier(fishingRoadGuid, (int)HarborLot.FISHING_RODS), new DiscordSelectComponentOption(
                    "Fishing Rods",
                    fishingRoadGuid,
                    emoji: new DiscordComponentEmoji(1201123346688004136)) },
                { new Identifier(fishesGuid, (int)HarborLot.FISHES), new DiscordSelectComponentOption(
                    $"Fishes",
                    fishesGuid,
                    emoji: new DiscordComponentEmoji(1201123133961277490)) },
                { new Identifier(itemsGuid, (int)HarborLot.ITEMS), new DiscordSelectComponentOption(
                    $"Items and Ores",
                    itemsGuid,
                    emoji: new DiscordComponentEmoji(1201123127359447180)) }
            };

            context.Client.ComponentInteractionCreated += async (client, args) =>
            {
                if (args.User.Id.ToString() != _user.UserId) return;

                foreach (var option in dropdrownOptions)
                {
                    if (args == null) continue;
                    if (!args.Values.Contains(option.Key.identifier)) continue;
                    if (args.Interaction == null) continue;

                    await args.Interaction.CreateResponseAsync(InteractionResponseType.UpdateMessage, new DiscordInteractionResponseBuilder()
                        .AddEmbed(GetHarborEmbed((HarborLot)option.Key.content))
                        .WithContent("> To buy item type /harborbuy [itemId]"));
                    return;
                }

                return;
            };


            return dropdrownOptions;
        }

        private DiscordEmbed GetHarborEmbed(HarborLot content)
        {
            switch (content)
            {
                case HarborLot.FISHING_RODS:
                    return new DiscordEmbedBuilder()
                    {
                        Color = DiscordColor.Red,
                        Title = "Fishing Rods",
                        Description = $"> {GetTotalShopStringByElements(ItemsDb.FishingRods)}",
                        ImageUrl = HarborBannerUrl
                    }.Build();

                case HarborLot.FISHES:
                    return new DiscordEmbedBuilder()
                    {
                        Color = DiscordColor.Red,
                        Title = "Fishes",
                        Description = $"> {GetTotalShopStringByElements(ItemsDb.Fishes)}",
                        ImageUrl = HarborBannerUrl
                    }.Build();

                case HarborLot.ITEMS:
                    return new DiscordEmbedBuilder()
                    {
                        Color = DiscordColor.Red,
                        Title = "Items and Ores",
                        Description = $"> {GetTotalShopStringByElements(ItemsDb.Items)}",
                        ImageUrl = HarborBannerUrl
                    }.Build();

                default:
                    return new DiscordEmbedBuilder()
                    {
                        Color = DiscordColor.Black,
                        Title = "0 lots found",
                        ImageUrl = HarborBannerUrl
                    }.Build();
            }
        }

        private string GetTotalShopStringByElements(ReadOnlyDictionary<int, UserItem> items)
        {
            var sb = new StringBuilder();
            foreach (var item in items.Values)
            {
                var buyPrice = item.BuyPrice;
                var sellPrice = item.SellPrice;
                sb.AppendLine($"{item.ItemId} | {item.EmojiCode} | BUY: {buyPrice.Mora}{Localization.MoraEmoji} {buyPrice.Primogems}{Localization.PrimosEmoji} | SELL: {sellPrice.Mora}{Localization.MoraEmoji} {sellPrice.Primogems}{Localization.PrimosEmoji} {sellPrice.Pills}{Localization.PillsEmoji}\n");
            }

            return sb.ToString();
        }

        public async Task<string> Buy(UserData user, DataHandler data, UserItem item)
        {
            if (user.Wallet.Mora < item.BuyPrice.Mora) return "> Not enough resources to buy this lot.";
            if (user.Wallet.Primogems < item.BuyPrice.Primogems) return "> Not enough resources to buy this lot.";
            if (user.Wallet.Pills < item.BuyPrice.Pills) return "> Not enough resources to buy this lot.";

            user.AddCurrency(new DiscordWallet(-item.BuyPrice.Primogems, -item.BuyPrice.Mora, -item.BuyPrice.Pills));

            await data.SendData(user, new UserUpdateQueryBuilder(user, QueryElement.Wallet, QueryElement.Stats, QueryElement.Inventory).Build());
            return string.Empty;
        }

        public async Task<string> Sell(UserData user, DataHandler data, UserItem item)
        {
            if (!user.Inventory.Contains(item)) return "Seems like you dont have this item";

            if (user.Stats.EquippedItem.ItemId == item.ItemId) user.Stats.EquippedItem = null;
            user.AddCurrency(new DiscordWallet(item.SellPrice.Primogems, item.SellPrice.Mora, item.SellPrice.Pills));

            await data.SendData(user, new UserUpdateQueryBuilder(user, QueryElement.Wallet, QueryElement.Stats, QueryElement.Inventory).Build());
            return string.Empty;
        }
    }
}
