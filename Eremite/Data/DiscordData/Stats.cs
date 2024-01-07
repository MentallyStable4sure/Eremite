
using Eremite.Data.Localization;

namespace Eremite.Data.DiscordData
{
    [Serializable]
    public class Stats
    {
        public int TimesWelkinWon = 0;
        public int TimesPulled = 0;
        public int TimesTraveled = 0;
        public int TimesDailiesCompleted = 0;

        public int TotalPrimogemsEarned = 0;
        public int TotalPrimogemsSpent = 0;
        public int TotalPillsEarned = 0;
        public int TotalPillsSpent = 0;

        public int LargestCashback = 0;
        public int TotalCashback = 0;
        public int TotalCharactersSacrificed = 0;

        public Language Language = Language.English;
        public string UserUID = string.Empty;
        public DateTime NotifyShowTimestamp = DateTime.MinValue;
    }
}
