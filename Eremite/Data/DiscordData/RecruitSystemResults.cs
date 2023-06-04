namespace Eremite.Data.DiscordData
{
    public class RecruitSystemResults
    {
        public EremiteRecruit RandomEremiteWon = null;
        public EremiteRecruit RandomVipEremiteWon = null;

        public List<EremiteRecruit> GuaranteedEremitesWon = null;
        public DateTime TimestampResults = DateTime.Now.ToUniversalTime();

        public string GetResultsShortDate() => TimestampResults.ToShortDateString();

        public RecruitSystemResults(EremiteRecruit randomEremite, EremiteRecruit randomVipEremite, List<EremiteRecruit> guaranteedEremites)
        {
            RandomEremiteWon = randomEremite;
            RandomVipEremiteWon = randomVipEremite;
            GuaranteedEremitesWon = guaranteedEremites;
        }
    }
}
