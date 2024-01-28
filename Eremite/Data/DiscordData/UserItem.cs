
namespace Eremite.Data.DiscordData
{
    [Serializable]
    public class UserItem
    {
        public int ItemId; 
        public string EmojiCode = string.Empty;

        public int Amount = 0;
        public Status StatusToGive = null;

        public UserItem(int itemId, string emojiCode, Status statusToGive, int amount = 1)
        {
            ItemId = itemId;
            EmojiCode = emojiCode;
            Amount = amount;
            StatusToGive = statusToGive;
        }
    }
}
