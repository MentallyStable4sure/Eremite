using Eremite.Services;
using Eremite.Data.DiscordData;

namespace Eremite.PerkActions
{
    public class LowerDailyCdTeamDependent : BasePerkAction
    {
        public LowerDailyCdTeamDependent()
        {
            PerkNeededToProc = Perk.LOWER_DAILY_COOLDOWN_TEAM_DEPENDENT;

            EventsWhereCanBeProced = new TimeGatedEventType[] { TimeGatedEventType.Daily };
        }

        protected override string OnProced(UserData user, DataHandler data, TimeGatedEvent eventProced, Award award)
        {
            PerksExtensions.LowerCooldownTeamDependent(user, eventProced.EventType, PerksExtensions.MinutesCooldownPerCharacterDaily);
            return additionalInfo;
        }
    }
}
