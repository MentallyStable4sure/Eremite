using Eremite.Services;
using Eremite.Data.DiscordData;

namespace Eremite.PerkActions
{
    public abstract class BasePerkAction : IPerkAction
    {
        public TimeGatedEventType[] EventsWhereCanBeProced { get; protected set; } = new TimeGatedEventType[0];

        public Perk PerkNeededToProc { get; protected set; } = Perk.NO_BUFF;

        protected string additionalInfo = string.Empty;

        public string DoAction(UserData user, DataHandler data, TimeGatedEvent eventProced, Award award)
        {
            additionalInfo = string.Empty;

            if (!EventsWhereCanBeProced.Contains(eventProced.EventType)) return additionalInfo;

            return OnProced(user, data, eventProced, award);
        }

        protected abstract string OnProced(UserData user, DataHandler data, TimeGatedEvent eventProced, Award award);
    }
}
