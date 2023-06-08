
namespace Eremite.Data.DiscordData
{
    [Serializable]
    public class Stats
    {
        public DateTime TimeLastTravel = DateTime.Now.AddDays(-5);
        public DateTime TimeLastTeapotVisit = DateTime.Now.AddDays(-5);
        public int TimesWelkinWon = 0;
        public int TimesPulled = 0;
        public int TimesTraveled = 0;
        public int TimesTeapotVisited = 0;
        public int TimesEremitesRecruitSystemEnrolled = 0;
        public int TotalPrimogemsEarned = 0;
        public int TotalPrimogemsSpent = 0;
        public int LargestCashback = 0;
        public int TotalCashback = 0;
    }
}
