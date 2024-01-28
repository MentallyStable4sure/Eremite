using Eremite.Services;
using Eremite.Data.DiscordData;

namespace Eremite.PerkActions
{
    public class LowerAdventureCdPermanent : BasePerkAction
    {
        public LowerAdventureCdPermanent()
        {
            PerkNeededToProc = Perk.LOWER_ADVENTURE_COOLDOWN_PERMANENT;

            EventsWhereCanBeProced = new TimeGatedEventType[] { TimeGatedEventType.Adventure };
        }

        protected override string OnProced(UserData user, DataHandler data, TimeGatedEvent eventProced, Award award)
        {
            PerksExtensions.LowerCooldownCustomHours(user, eventProced.EventType, PerksExtensions.HoursCooldownAdventurePermanent);
            return additionalInfo;
        }
    }
}
