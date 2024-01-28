using Eremite.Services;
using Eremite.Data.DiscordData;

namespace Eremite.PerkActions
{
    public class DoublePillsDaily : BasePerkAction
    {
        public DoublePillsDaily()
        {
            PerkNeededToProc = Perk.DOUBLE_PILLS_DAILY;

            EventsWhereCanBeProced = new TimeGatedEventType[] { TimeGatedEventType.Daily };
        }

        protected override string OnProced(UserData user, DataHandler data, TimeGatedEvent eventProced, Award award)
        {
            int baseAmount = award.CurrenciesToAdd.Pills;
            award.MultiplyPills(2);
            additionalInfo = $"> {baseAmount}{Localization.PillsEmoji} x2 => {award.CurrenciesToAdd.Pills}{Localization.PillsEmoji}";
            return additionalInfo;
        }
    }
}
