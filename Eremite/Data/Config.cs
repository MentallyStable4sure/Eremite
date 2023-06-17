namespace Eremite.Data
{
    [Serializable]
    public class Config
    {
        public int PullCost = 160;

        //chances
        public Dictionary<int, int> Chances = new Dictionary<int, int>(); //KEY:Value eg. STARS:Chance

        //URLs
        public string DefaultAkashaImageUrl = "https://raw.githubusercontent.com/MentallyStable4sure/Eremite/main/content/characters/nochar.png";
    }
}
