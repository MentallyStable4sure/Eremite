using Eremite.Services;
using Eremite.Data.DiscordData;

namespace Eremite.PerkActions
{
    public class ConvertPrimosIntoMoraAll1To1 : BasePerkAction
    {
        public ConvertPrimosIntoMoraAll1To1()
        {
            PerkNeededToProc = Perk.CONVERT_PRIMOS_INTO_MORA_ALL_1TO1;

            EventsWhereCanBeProced = new TimeGatedEventType[] { TimeGatedEventType.Adventure, TimeGatedEventType.Daily };
        }

        protected override string OnProced(UserData user, DataHandler data, TimeGatedEvent eventProced, Award award)
        {
            int baseAmount = award.CurrenciesToAdd.Primogems;
            int convertion = PerksExtensions.ConvertPrimosToMora(award, 1);
            additionalInfo = $"> {baseAmount}{Localization.PrimosEmoji} => {convertion}{Localization.MoraEmoji}";
            return additionalInfo;
        }
    }
}
