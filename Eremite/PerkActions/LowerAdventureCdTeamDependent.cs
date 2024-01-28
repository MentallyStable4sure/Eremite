using Eremite.Services;
using Eremite.Data.DiscordData;

namespace Eremite.PerkActions
{
    public class LowerAdventureCdTeamDependent : BasePerkAction
    {
        public LowerAdventureCdTeamDependent()
        {
            PerkNeededToProc = Perk.LOWER_ADVENTURE_COOLDOWN_TEAM_DEPENDENT;

            EventsWhereCanBeProced = new TimeGatedEventType[] { TimeGatedEventType.Adventure };
        }

        protected override string OnProced(UserData user, DataHandler data, TimeGatedEvent eventProced, Award award)
        {
            PerksExtensions.LowerCooldownTeamDependent(user, eventProced.EventType, PerksExtensions.MinutesCooldownPerCharacterAdventure);
            return additionalInfo;
        }
    }
}
