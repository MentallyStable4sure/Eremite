using Eremite.Services;
using Eremite.Data.DiscordData;

namespace Eremite.PerkActions
{
    public class ConvertPrimosIntoPillsAll1To2NoMora : BasePerkAction
    {
        public ConvertPrimosIntoPillsAll1To2NoMora()
        {
            PerkNeededToProc = Perk.CONVERT_PRIMOS_INTO_PILLS_ALL_1TO2_NO_MORA;

            EventsWhereCanBeProced = new TimeGatedEventType[] { TimeGatedEventType.Adventure, TimeGatedEventType.Daily, TimeGatedEventType.Fishblasting };
        }

        protected override string OnProced(UserData user, DataHandler data, TimeGatedEvent eventProced, Award award)
        {
            int baseAmount = award.CurrenciesToAdd.Primogems;
            int convertion = PerksExtensions.ConvertPrimosToPills(award, 2);
            award.CurrenciesToAdd.Mora = 0;
            additionalInfo = $"> {baseAmount}{Localization.PrimosEmoji} => {convertion}{Localization.PillsEmoji}";
            return additionalInfo;
        }
    }
}
