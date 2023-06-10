
namespace Eremite.Data.DiscordData
{
    [Serializable]
    public class UserData
    {
        public string UserId; //snowflake id  prob
        public string Username = string.Empty;
        public Character EquippedCharacter;

        public DiscordWallet Wallet = new DiscordWallet(); //users money
        public List<Character> Characters = new List<Character>(); //users inventory
        public List<TimeGatedEvent> Events = new List<TimeGatedEvent>(); //events user participated in with dateTimes

        public Stats Stats = new Stats(); //all the stats about visits/etc.
    }
}
