using Eremite.Services;
using Eremite.Data.DiscordData;

namespace Eremite.PerkActions
{
    public class OnSacrifice10PillPerChar : BasePerkAction
    {
        public OnSacrifice10PillPerChar()
        {
            PerkNeededToProc = Perk.ON_SACRIFICE_GIVES_10_PILLS_PER_CHAR_IN_INVENTORY;

            EventsWhereCanBeProced = new TimeGatedEventType[] { TimeGatedEventType.Sacrifice };
        }

        protected override string OnProced(UserData user, DataHandler data, TimeGatedEvent eventProced, Award award)
        {
            PerksExtensions.GiveAwardPerChar(user, award, new Award(new DiscordWallet(0, 0, 10)));
            additionalInfo = $"> ➕{10 * user.Characters.Count}{Localization.PillsEmoji}";
            return additionalInfo;
        }
    }
}
