using Eremite.Services;
using Eremite.Data.DiscordData;

namespace Eremite.PerkActions
{
    public class LowerDailyCdPermanent : BasePerkAction
    {
        public LowerDailyCdPermanent()
        {
            PerkNeededToProc = Perk.LOWER_DAILY_COOLDOWN_PERMANENT;

            EventsWhereCanBeProced = new TimeGatedEventType[] { TimeGatedEventType.Daily };
        }

        protected override string OnProced(UserData user, DataHandler data, TimeGatedEvent eventProced, Award award)
        {
            PerksExtensions.LowerCooldownCustomHours(user, eventProced.EventType, PerksExtensions.HoursCooldownDailyPermanent);
            return additionalInfo;
        }
    }
}
