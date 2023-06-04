namespace Eremite.Data.DiscordData
{
    [Serializable]
    public class RecruitSystemResultsDatabase
    {
        public RecruitSystemResults LatestResult = null;
        public List<RecruitSystemResults> ResultsHistory = null;
        public DateTime TimestampLastResults = DateTime.Now;
    }
}
