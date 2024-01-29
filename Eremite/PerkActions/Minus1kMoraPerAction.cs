using Eremite.Services;
using Eremite.Data.DiscordData;

namespace Eremite.PerkActions
{
    public class Minus1kMoraPerAction : BasePerkAction
    {
        public Minus1kMoraPerAction()
        {
            PerkNeededToProc = Perk.MINUS1000MORA_PER_SACRIFICE;

            EventsWhereCanBeProced = new TimeGatedEventType[] { TimeGatedEventType.Sacrifice };
        }

        protected override string OnProced(UserData user, DataHandler data, TimeGatedEvent eventProced, Award award)
        {
            if (user.Wallet.Mora < 1000) return additionalInfo;
            user.Wallet.Mora -= 1000;
            return additionalInfo;
        }
    }
}
