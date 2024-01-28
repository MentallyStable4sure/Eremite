using Eremite.Services;
using Eremite.Data.DiscordData;

namespace Eremite.PerkActions
{
    public interface IPerkAction
    {
        public TimeGatedEventType[] EventsWhereCanBeProced { get; }

        public Perk PerkNeededToProc { get; }

        public string DoAction(UserData user, DataHandler data, TimeGatedEvent eventProced, Award award);
    }
}
