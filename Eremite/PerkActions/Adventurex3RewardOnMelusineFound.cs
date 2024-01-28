using Eremite.Services;
using Eremite.Data.DiscordData;

namespace Eremite.PerkActions
{
    public class Adventurex3RewardOnMelusineFound : BasePerkAction
    {
        public Adventurex3RewardOnMelusineFound()
        {
            PerkNeededToProc = Perk.ADVENTURE_X3_REWARD_WHEN_MELUSINE_FOUND;

            EventsWhereCanBeProced = new TimeGatedEventType[] { TimeGatedEventType.Adventure };
        }

        protected override string OnProced(UserData user, DataHandler data, TimeGatedEvent eventProced, Award award)
        {
            if (eventProced.Melusines <= 0) return additionalInfo;
            award.MultiplyAward(3);
            return additionalInfo;
        }
    }
}
