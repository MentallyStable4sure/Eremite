using Eremite.Actions;
using Eremite.Data.DiscordData;

namespace Eremite.PerkActions
{
    public static class PerksExtensions
    {
        public const int MinutesCooldownPerCharacterAdventure = 10;
        public const int MinutesCooldownPerCharacterDaily = 30;
        public const int ChancesPerCharacterResetCDAdventure = 5;

        public const float PercentageMaxReset = 0.75f; //75%

        public const int HoursCooldownAdventurePermanent = 1;
        public const int HoursCooldownDailyPermanent = 12;

        public static void GiveAwardPerChar(UserData user, Award award, Award rewardPerCharacter)
        {
            var charsCount = user.Characters.Count;

            award.CurrenciesToAdd.Mora += (charsCount * rewardPerCharacter.CurrenciesToAdd.Mora);
            award.CurrenciesToAdd.Primogems += (charsCount * rewardPerCharacter.CurrenciesToAdd.Primogems);
            award.CurrenciesToAdd.Pills += (charsCount * rewardPerCharacter.CurrenciesToAdd.Pills);
        }

        public static void MultiplyAward(this Award award, int multiplier)
        {
            MultiplyMora(award,multiplier);
            MultiplyPrimos(award, multiplier);
            MultiplyPills(award, multiplier);
        }

        public static void MultiplyMora(this Award award, int multiplier = 2) => award.CurrenciesToAdd.Mora *= multiplier;
        public static void MultiplyPrimos(this Award award, int multiplier = 2) => award.CurrenciesToAdd.Primogems *= multiplier;
        public static void MultiplyPills(this Award award, int multiplier = 2) => award.CurrenciesToAdd.Pills *= multiplier;

        public static void LowerCooldownTeamDependent(UserData user, TimeGatedEventType eventType, int minutesPerCharacter)
        {
            int minutesCooldownDecrease = -1 * (minutesPerCharacter * user.Characters.Count);
            var timeGatedEvent = TimeGatedAction.GetPreviousEventByType(user, eventType);
            var percentageMax = (int)(timeGatedEvent.TimeBetweenTriggers.TotalMinutes * PercentageMaxReset);
            minutesCooldownDecrease = minutesCooldownDecrease > percentageMax ? percentageMax : minutesCooldownDecrease;
            timeGatedEvent.LastTimeTriggered = timeGatedEvent.LastTimeTriggered.AddMinutes(minutesCooldownDecrease);
        }

        public static void LowerCooldownCustomHours(UserData user, TimeGatedEventType eventType, int hours)
        {
            int hoursCooldownDecrease = -1 * hours;
            var timeGatedEvent = TimeGatedAction.GetPreviousEventByType(user, eventType);
            var percentageMax = (int)(timeGatedEvent.TimeBetweenTriggers.TotalHours * 1);
            hoursCooldownDecrease = hoursCooldownDecrease > percentageMax ? percentageMax : hoursCooldownDecrease;
            timeGatedEvent.LastTimeTriggered = timeGatedEvent.LastTimeTriggered.AddHours(hoursCooldownDecrease);
        }

        public static int ConvertMoraToPrimos(this Award award, int ratio = 2)
        {
            if (award.CurrenciesToAdd.Mora < ratio) return 0;

            int converted = (int)(award.CurrenciesToAdd.Mora / ratio);
            award.CurrenciesToAdd.Primogems += converted;
            award.CurrenciesToAdd.Mora = 0;

            return converted;
        }
        public static int ConvertPrimosToMora(this Award award, int ratio = 2)
        {
            if (award.CurrenciesToAdd.Primogems < ratio) return 0;

            int converted = (int)(award.CurrenciesToAdd.Primogems / ratio);
            award.CurrenciesToAdd.Mora += converted;
            award.CurrenciesToAdd.Primogems = 0;

            return converted;
        }

        public static int ConvertPrimosToPills(this Award award, int ratio = 2)
        {
            if (award.CurrenciesToAdd.Primogems < ratio) return 0;

            int converted = (int)(award.CurrenciesToAdd.Primogems / ratio);
            award.CurrenciesToAdd.Pills += converted;
            award.CurrenciesToAdd.Primogems = 0;

            return converted;
        }
    }
}
