using Eremite.Services;
using Eremite.Data.DiscordData;

namespace Eremite.PerkActions
{
    public class ConvertPrimosIntoPillsAll1To2 : BasePerkAction
    {
        public ConvertPrimosIntoPillsAll1To2()
        {
            PerkNeededToProc = Perk.CONVERT_PRIMOS_INTO_PILLS_ALL_1TO2;

            EventsWhereCanBeProced = new TimeGatedEventType[] { TimeGatedEventType.Adventure, TimeGatedEventType.Daily };
        }

        protected override string OnProced(UserData user, DataHandler data, TimeGatedEvent eventProced, Award award)
        {
            int baseAmount = award.CurrenciesToAdd.Primogems;
            int convertion = PerksExtensions.ConvertPrimosToPills(award, 2);
            additionalInfo = $"> {baseAmount}{Localization.PrimosEmoji} => {convertion}{Localization.PillsEmoji}";
            return additionalInfo;
        }
    }
}
