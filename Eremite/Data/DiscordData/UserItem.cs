
namespace Eremite.Data.DiscordData
{
    [Serializable]
    public class UserItem
    {
        public int ItemId; 
        public string EmojiCode = string.Empty;

        public int Amount = 0;
        public Status StatusToGive = null;
        public DiscordWallet SellPrice = new DiscordWallet();
        public DiscordWallet BuyPrice = new DiscordWallet();

        public UserItem(int itemId, string emojiCode, Status statusToGive, DiscordWallet sellPrice = null, DiscordWallet buyPrice = null, int amount = 1)
        {
            ItemId = itemId;
            EmojiCode = emojiCode;
            Amount = amount;
            StatusToGive = statusToGive;

            SellPrice = sellPrice == null ? new DiscordWallet() : sellPrice;
            BuyPrice = buyPrice == null ? new DiscordWallet() : buyPrice;
        }
    }
}
