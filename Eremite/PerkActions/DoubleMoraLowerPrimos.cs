using Eremite.Services;
using Eremite.Data.DiscordData;

namespace Eremite.PerkActions
{
    public class DoubleMoraLowerPrimos : BasePerkAction
    {
        public DoubleMoraLowerPrimos()
        {
            PerkNeededToProc = Perk.DOUBLE_MORA_LOWER_PRIMOS;

            EventsWhereCanBeProced = new TimeGatedEventType[] { TimeGatedEventType.Adventure, TimeGatedEventType.Daily };
        }

        protected override string OnProced(UserData user, DataHandler data, TimeGatedEvent eventProced, Award award)
        {
            int baseAmount = award.CurrenciesToAdd.Mora;
            award.MultiplyMora(2);
            award.CurrenciesToAdd.Primogems = award.CurrenciesToAdd.Primogems > 0 ? award.CurrenciesToAdd.Primogems / 2 : 0;
            additionalInfo = $"> {baseAmount}{Localization.MoraEmoji} x2 => {award.CurrenciesToAdd.Mora}{Localization.MoraEmoji}";
            return additionalInfo;
        }
    }
}
