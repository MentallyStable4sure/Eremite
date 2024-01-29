using Eremite.Services;
using Eremite.Data.DiscordData;

namespace Eremite.PerkActions
{
    public class DoublePrimosLowerMora : BasePerkAction
    {
        public DoublePrimosLowerMora()
        {
            PerkNeededToProc = Perk.DOUBLE_PRIMOS_LOWER_MORA;

            EventsWhereCanBeProced = new TimeGatedEventType[] { TimeGatedEventType.Adventure, TimeGatedEventType.Daily, TimeGatedEventType.Fishblasting };
        }

        protected override string OnProced(UserData user, DataHandler data, TimeGatedEvent eventProced, Award award)
        {
            int baseAmount = award.CurrenciesToAdd.Primogems;
            award.MultiplyPrimos(2);
            award.CurrenciesToAdd.Mora = award.CurrenciesToAdd.Mora > 0 ? award.CurrenciesToAdd.Mora / 2 : 0;
            additionalInfo = $"> {baseAmount}{Localization.PrimosEmoji} x2 => {award.CurrenciesToAdd.Primogems}{Localization.PrimosEmoji}";
            return additionalInfo;
        }
    }
}
