namespace Eremite.Data
{
    [Serializable]
    public class Config
    {
        public int PullCost = 160;
        public int MaxCashback = 3001; //Max Mora cashback

        //chances
        public Dictionary<int, int> Chances = new Dictionary<int, int>(); //KEY:Value eg. STARS:Chance

        //URLs
        public string DefaultAkashaImageUrl = "https://raw.githubusercontent.com/MentallyStable4sure/Eremite/main/content/characters/nochar.png";

        public string NotifyOfTheDay = "";
        public string NotifyImage = "https://raw.githubusercontent.com/MentallyStable4sure/Eremite/main/content/info.gif";
        public TimeSpan NotifyOfTheDayCooldown = TimeSpan.FromHours(12);
    }
}
