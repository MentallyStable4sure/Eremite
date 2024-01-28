
namespace Eremite.Data.DiscordData
{
    public class FishblastingEvent : TimeGatedEvent
    {
        public List<int> FishesCanBeFound = new List<int>();
        public string ButtonText;
        public string ButtonGuid;

        public FishblastingEvent(TimeGatedEventType type, TimeSpan timeBetweenTriggers, Award customAward = null, int melusines = 0) : base(type, timeBetweenTriggers, customAward, melusines)
        {
        }
    }
}
