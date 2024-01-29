using Eremite.Services;
using Eremite.Data.DiscordData;

namespace Eremite.PerkActions
{
    public class DoubleMora : BasePerkAction
    {
        public DoubleMora()
        {
            PerkNeededToProc = Perk.DOUBLE_MORA;

            EventsWhereCanBeProced = new TimeGatedEventType[] { TimeGatedEventType.Adventure, TimeGatedEventType.Daily, TimeGatedEventType.Fishblasting };
        }

        protected override string OnProced(UserData user, DataHandler data, TimeGatedEvent eventProced, Award award)
        {
            int baseAmount = award.CurrenciesToAdd.Mora;
            award.MultiplyMora(2);
            additionalInfo = $"> {baseAmount}{Localization.MoraEmoji} x2 => {award.CurrenciesToAdd.Mora}{Localization.MoraEmoji}";
            return additionalInfo;
        }
    }
}
