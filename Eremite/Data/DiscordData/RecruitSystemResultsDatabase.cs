namespace Eremite.Data.DiscordData
{
    [Serializable]
    public class RecruitSystemResultsDatabase
    {
        public RecruitSystemResults LatestResult;
        public List<RecruitSystemResults> ResultsHistory;
        public DateTime TimestampLastResults = DateTime.Now;
    }
}
