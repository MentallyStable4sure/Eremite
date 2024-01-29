using Eremite.Services;
using Eremite.Data.DiscordData;

namespace Eremite.PerkActions
{
    public class ConvertMoraIntoPrimosAll1To1 : BasePerkAction
    {
        public ConvertMoraIntoPrimosAll1To1()
        {
            PerkNeededToProc = Perk.CONVERT_MORA_INTO_PRIMOGEMS_ALL_1TO1;

            EventsWhereCanBeProced = new TimeGatedEventType[] { TimeGatedEventType.Adventure, TimeGatedEventType.Daily, TimeGatedEventType.Fishblasting };
        }

        protected override string OnProced(UserData user, DataHandler data, TimeGatedEvent eventProced, Award award)
        {
            int baseAmount = award.CurrenciesToAdd.Mora;
            int convertion = PerksExtensions.ConvertMoraToPrimos(award, 1);
            additionalInfo = $"> {baseAmount}{Localization.MoraEmoji} => {convertion}{Localization.PrimosEmoji}";
            return additionalInfo;
        }
    }
}
