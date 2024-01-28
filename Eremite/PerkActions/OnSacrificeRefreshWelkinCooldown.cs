using Eremite.Services;
using Eremite.Data.DiscordData;

namespace Eremite.PerkActions
{
    public class OnSacrificeRefreshWelkinCooldown : BasePerkAction
    {
        public OnSacrificeRefreshWelkinCooldown()
        {
            PerkNeededToProc = Perk.WHEN_SACRIFICED_REFRESHES_WELKIN_COOLDOWN;

            EventsWhereCanBeProced = new TimeGatedEventType[] { TimeGatedEventType.Sacrifice };
        }

        protected override string OnProced(UserData user, DataHandler data, TimeGatedEvent eventProced, Award award)
        {
            foreach (var userEvent in user.Events)
            {
                if (userEvent.EventType != TimeGatedEventType.Welkin) continue;

                userEvent.TimeBetweenTriggers = TimeSpan.FromSeconds(1);
            }
            additionalInfo = $"> {Localization.WelkinEmoji} 00:00:00 {Localization.WelkinEmoji}";
            return additionalInfo;
        }
    }
}
