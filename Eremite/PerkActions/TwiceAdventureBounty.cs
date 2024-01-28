using Eremite.Services;
using Eremite.Data.DiscordData;

namespace Eremite.PerkActions
{
    public class TwiceAdventureBounty : BasePerkAction
    {
        public TwiceAdventureBounty()
        {
            PerkNeededToProc = Perk.TWICE_ADVENTURE_BOUNTY;

            EventsWhereCanBeProced = new TimeGatedEventType[] { TimeGatedEventType.Adventure };
        }

        protected override string OnProced(UserData user, DataHandler data, TimeGatedEvent eventProced, Award award)
        {
            award.MultiplyAward(2);
            return additionalInfo;
        }
    }
}
