using Eremite.Services;
using Eremite.Data.DiscordData;

namespace Eremite.PerkActions
{
    public class ConvertMoraIntoPrimosAdventure1To2 : BasePerkAction
    {
        public ConvertMoraIntoPrimosAdventure1To2()
        {
            PerkNeededToProc = Perk.CONVERT_MORA_INTO_PRIMOGEMS_ADVENTURE_1TO2;

            EventsWhereCanBeProced = new TimeGatedEventType[] { TimeGatedEventType.Adventure };
        }

        protected override string OnProced(UserData user, DataHandler data, TimeGatedEvent eventProced, Award award)
        {
            int baseAmount = award.CurrenciesToAdd.Mora;
            int convertion = PerksExtensions.ConvertMoraToPrimos(award, 2);
            additionalInfo = $"> {baseAmount}{Localization.MoraEmoji} => {convertion}{Localization.PrimosEmoji}";
            return additionalInfo;
        }
    }
}
