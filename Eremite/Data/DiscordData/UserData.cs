
namespace Eremite.Data.DiscordData
{
    [Serializable]
    public class UserData
    {
        public string UserId; //snowflake id  prob
        public string Username = string.Empty;
        public int EquippedCharacter = UserExtensions.UnsetId;

        public DiscordWallet Wallet = new DiscordWallet(); //users money
        public List<int> Characters = new List<int>(); //users characters id (owned chars basically)
        public List<TimeGatedEvent> Events = new List<TimeGatedEvent>(); //events user participated in with dateTimes

        public Stats Stats = new Stats(); //all the stats about visits/etc.
    }
}
