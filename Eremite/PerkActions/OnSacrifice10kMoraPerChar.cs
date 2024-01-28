using Eremite.Services;
using Eremite.Data.DiscordData;

namespace Eremite.PerkActions
{
    public class OnSacrifice10kMoraPerChar : BasePerkAction
    {
        public OnSacrifice10kMoraPerChar()
        {
            PerkNeededToProc = Perk.ON_SACRIFICE_GIVES_10000_MORA_PER_CHAR_IN_INVENTORY;

            EventsWhereCanBeProced = new TimeGatedEventType[] { TimeGatedEventType.Sacrifice };
        }

        protected override string OnProced(UserData user, DataHandler data, TimeGatedEvent eventProced, Award award)
        {
            PerksExtensions.GiveAwardPerChar(user, award, new Award(new DiscordWallet(0, 10000)));
            additionalInfo = $"> ➕{10000 * user.Characters.Count}{Localization.MoraEmoji}";
            return additionalInfo;
        }
    }
}
