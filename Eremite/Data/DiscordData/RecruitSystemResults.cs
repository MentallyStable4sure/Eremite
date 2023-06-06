using Eremite.Data.GenshinData;

namespace Eremite.Data.DiscordData
{
    public class RecruitSystemResults
    {
        public EremiteRecruit RandomEremiteWon;
        public EremiteRecruit RandomVipEremiteWon;

        public List<EremiteRecruit> GuaranteedEremitesWon;
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
