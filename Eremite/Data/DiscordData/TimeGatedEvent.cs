
namespace Eremite.Data.DiscordData
{
    [Serializable]
    public class TimeGatedEvent
    {
        public TimeGatedEventType EventType = TimeGatedEventType.None;
        public DateTime LastTimeTriggered = DateTime.UtcNow.AddMonths(-2);
        public TimeSpan TimeBetweenTriggers;
        public string ImageUrl;
        public int Melusines = 0;

        public int TimesTicked = 0;

        public Award Award = new Award(new DiscordWallet(0, 0, 0)); //Default award for the event participation

        public TimeGatedEvent(TimeGatedEventType type, TimeSpan timeBetweenTriggers, Award customAward = null, int melusines = 0)
        {
            EventType = type;
            TimeBetweenTriggers = timeBetweenTriggers;
            if (customAward != null) Award = customAward;

            LastTimeTriggered = DateTime.UtcNow.AddMonths(-2);
            Melusines = melusines;
        }
    }
}
