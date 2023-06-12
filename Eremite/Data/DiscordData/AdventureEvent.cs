﻿
namespace Eremite.Data.DiscordData
{
    internal class AdventureEvent : TimeGatedEvent
    {
        public Region Region;
        public string ButtonText;
        public string ButtonGuid;

        public AdventureEvent(TimeGatedEventType type, TimeSpan timeBetweenTriggers, Award customAward = null) : base(type, timeBetweenTriggers, customAward)
        {
        }
    }
}
