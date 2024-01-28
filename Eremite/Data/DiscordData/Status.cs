using Eremite.PerkActions;
using Eremite.Services;

namespace Eremite.Data.DiscordData
{
    [Serializable]
    public class Status
    {
        public Award AwardToGiveOnUse = new Award(new DiscordWallet());
        public Perk PerkNeeded = Perk.NO_BUFF;
        public TimeGatedEventType EventTypeToProc = TimeGatedEventType.None;
        public TimeGatedEventType[] EventCooldownDecrease = new TimeGatedEventType[0];
        public TimeSpan TimeToDecrease = TimeSpan.Zero;

        public bool IsDestroyable = true;
        public bool IsImmediateUse = false;

        public Status(Award awardToGiveOnUse, Perk perkNeeded = Perk.NO_BUFF, TimeGatedEventType eventTypeToProc = TimeGatedEventType.None, bool isDestroyable = true, bool isImmediateUse = true, TimeGatedEventType[] eventCooldownDecrease = default, TimeSpan timeToDecrease = default)
        {
            AwardToGiveOnUse = awardToGiveOnUse;
            PerkNeeded = perkNeeded;
            EventTypeToProc = eventTypeToProc;
            IsDestroyable = isDestroyable;
            IsImmediateUse = isImmediateUse;
            EventCooldownDecrease = eventCooldownDecrease == null ? new TimeGatedEventType[0] : eventCooldownDecrease;
            TimeToDecrease = timeToDecrease;
        }
    }
}
