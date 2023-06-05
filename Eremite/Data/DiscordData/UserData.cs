
namespace Eremite.Data.DiscordData
{
    [Serializable]
    public class UserData
    {
        public string UserId; //snowflake id  prob
        public string Username = string.Empty;
        public Character CurrentEquippedCharacter;

        public DiscordWallet Wallet = new DiscordWallet(); //users money
        public List<Character> Characters = new List<Character>(); //users inventory
        public List<Badge> Badges = new List<Badge>(); //player badges for future mini-games/etc.

        public DateTime TimeLastTravel = DateTime.Now.AddDays(-5);
        public DateTime TimeLastTeapotVisit = DateTime.Now.AddDays(-5);
        public int TimesWelkinWon = 0;
        public int TimesPulled = 0;
        public int TimesTraveled = 0;
        public int TimesTeapotVisited = 0;
        public int TimesEremitesRecruitSystemEnrolled = 0;

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

        public void AddCurrency(int primos, int mora)
        {
            Wallet.Primogems += primos;
            Wallet.Mora += mora;
        }
    }
}
