using Eremite.Services;
using Eremite.Data.DiscordData;

namespace Eremite.PerkActions
{
    public class DoublePrimos : BasePerkAction
    {
        public DoublePrimos()
        {
            PerkNeededToProc = Perk.DOUBLE_PRIMOS;

            EventsWhereCanBeProced = new TimeGatedEventType[] { TimeGatedEventType.Adventure, TimeGatedEventType.Daily };
        }

        protected override string OnProced(UserData user, DataHandler data, TimeGatedEvent eventProced, Award award)
        {
            int baseAmount = award.CurrenciesToAdd.Primogems;
            award.MultiplyPrimos(2);
            additionalInfo = $"> {baseAmount}{Localization.PrimosEmoji} x2 => {award.CurrenciesToAdd.Primogems}{Localization.PrimosEmoji}";
            return additionalInfo;
        }
    }
}
