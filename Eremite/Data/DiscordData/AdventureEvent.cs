
namespace Eremite.Data.DiscordData
{
    public class AdventureEvent : TimeGatedEvent
    {
        public Region Region;
        public string ButtonText;
        public string ButtonGuid;

        public AdventureEvent(TimeGatedEventType type, TimeSpan timeBetweenTriggers, Award customAward = null, int melusines = 0) : base(type, timeBetweenTriggers, customAward, melusines)
        {
        }
    }
}
