
namespace Eremite.Data.DiscordData
{
    [Serializable]
    public class TimeGatedEvent
    {
        public TimeGatedEventType EventType;
        public DateTime LastTimeTriggered;
        public TimeSpan TimeBetweenTriggers;
        public string ImageUrl;

        public int TimesTicked = 0;

        public Award Award = new Award(new DiscordWallet(200,200)); //Default award for the event participation

        public TimeGatedEvent(TimeGatedEventType type, TimeSpan timeBetweenTriggers, Award customAward = null)
        {
            EventType = type;
            TimeBetweenTriggers = timeBetweenTriggers;
            if (customAward != null) Award = customAward;

            LastTimeTriggered = DateTime.UtcNow.AddMonths(-1);
        }
    }
}
