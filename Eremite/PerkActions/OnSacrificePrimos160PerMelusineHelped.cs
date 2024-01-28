using Eremite.Services;
using Eremite.Data.DiscordData;

namespace Eremite.PerkActions
{
    public class OnSacrificePrimos160PerMelusineHelped : BasePerkAction
    {
        public const int MAX_PRIMOS_FOR_ALL_MELUSINES_HELPED = 10000;

        public OnSacrificePrimos160PerMelusineHelped()
        {
            PerkNeededToProc = Perk.ON_SACRIFICE_PRIMOS_160_PER_MELUSINE_HELPED;

            EventsWhereCanBeProced = new TimeGatedEventType[] { TimeGatedEventType.Sacrifice };
        }

        protected override string OnProced(UserData user, DataHandler data, TimeGatedEvent eventProced, Award award)
        {
            int amountToAdd = user.Stats.MelusinesHelped <= 0 ? 160 : 160 * user.Stats.MelusinesHelped;
            amountToAdd = amountToAdd <= MAX_PRIMOS_FOR_ALL_MELUSINES_HELPED ? amountToAdd : MAX_PRIMOS_FOR_ALL_MELUSINES_HELPED;

            award.CurrenciesToAdd.Primogems += amountToAdd;
            additionalInfo = $"> 160{Localization.PrimosEmoji}*{Localization.MelusineEmoji} => {160*user.Stats.MelusinesHelped}{Localization.MelusineEmoji}";
            return additionalInfo;
        }
    }
}
