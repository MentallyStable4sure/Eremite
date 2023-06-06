
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

        public Stats Stats = new Stats(); //all the stats about visits/etc.

        public void AddPulledCharacter(Character character)
        {
            Character duplicate;

            if(Characters.Count <= 0)
            {
                Characters.Add(character);
                return;
            }

            duplicate = Characters.Find(characterSaved => characterSaved.CharacterName == character.CharacterName);

            if (duplicate != null && duplicate.StarsRarity >= 10) Characters.Add(duplicate);
            if (duplicate == null) Characters.Add(character);
        }

        public void ResetWallet() => Wallet = new DiscordWallet();

        public void ResetStats() => Stats = new Stats();

        public void AddCurrency(int primos, int mora)
        {
            Wallet.Primogems += primos;
            Wallet.Mora += mora;
        }
    }
}
