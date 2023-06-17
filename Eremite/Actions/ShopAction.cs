using DSharpPlus.Entities;

namespace Eremite.Actions
{
    public class ShopAction
    {
        public const string DoriShopBannerUrl = "https://raw.githubusercontent.com/MentallyStable4sure/Eremite/main/content/events/Shop.jpg";

        public static DiscordEmbedBuilder GetEmbedWithShopInfo()
        {
            return new DiscordEmbedBuilder()
            {
                Color = DiscordColor.Purple,
                Title = "Dori's --jaw droppin prices-- brilliant shop!",
                ImageUrl = DoriShopBannerUrl,
            };
        }
    }
}
