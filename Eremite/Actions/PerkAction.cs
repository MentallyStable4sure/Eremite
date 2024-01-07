using Eremite.Services;
using Eremite.Data.DiscordData;

namespace Eremite.Actions
{
    public class PerkAction
    {
        public const int MinutesCooldownPerCharacterAdventure = 10;
        public const int MinutesCooldownPerCharacterDaily = 30;
        public const int ChancesPerCharacterResetCDAdventure = 5;

        public const float PercentageMaxReset = 0.75f; //75%

        public const int HoursCooldownAdventurePermanent = 1;
        public const int HoursCooldownDailyPermanent = 12;

        public static void ApplyPerk(UserData user, TimeGatedEventType eventType, Award award)
        {
            if (!user.IsAnyCharacterEquipped()) return;
            var perk = (Perk)(CharactersHandler.ConvertId(user.EquippedCharacter).PerkStat);
            Console.WriteLine($"Applying perk from user {user.Username}, event type: {eventType.ToString()}, equipped char: {user.EquippedCharacter}, perk: {perk}");

            switch (perk)
            {
                case Perk.NO_BUFF:
                    break;

                case Perk.DOUBLE_MORA_LOWER_PRIMOS:
                    DoubleMora(award);
                    award.CurrenciesToAdd.Primogems = award.CurrenciesToAdd.Primogems > 0 ? award.CurrenciesToAdd.Primogems / 2 : 0;
                    break;

                case Perk.DOUBLE_MORA:
                    DoubleMora(award);
                    break;

                case Perk.DOUBLE_PRIMOS_LOWER_MORA:
                    DoublePrimos(award);
                    award.CurrenciesToAdd.Mora = award.CurrenciesToAdd.Mora > 0 ? award.CurrenciesToAdd.Mora / 2 : 0;
                    break;

                case Perk.DOUBLE_PRIMOS:
                    DoublePrimos(award);
                    break;

                case Perk.TWICE_ADVENTURE_BOUNTY:
                    if (eventType != TimeGatedEventType.Adventure) break;
                    DoubleAward(award);
                    break;

                case Perk.LOWER_ADVENTURE_COOLDOWN_TEAM_DEPENDENT:
                    if (eventType != TimeGatedEventType.Adventure) break;
                    LowerCooldownTeamDependent(user, eventType, MinutesCooldownPerCharacterAdventure);
                    break;

                case Perk.LOWER_DAILY_COOLDOWN_TEAM_DEPENDENT:
                    if (eventType != TimeGatedEventType.Daily) break;
                    LowerCooldownTeamDependent(user, eventType, MinutesCooldownPerCharacterDaily);
                    break;

                case Perk.LOWER_ADVENTURE_COOLDOWN_PERMANENT:
                    if (eventType != TimeGatedEventType.Adventure) break;
                    LowerCooldownCustomHours(user, eventType, HoursCooldownAdventurePermanent);
                    break;

                case Perk.LOWER_DAILY_COOLDOWN_PERMANENT:
                    if ( eventType != TimeGatedEventType.Daily) break;
                    LowerCooldownCustomHours(user, eventType, HoursCooldownDailyPermanent);
                    break;

                case Perk.DOUBLE_PILLS_DAILY:
                    if (eventType != TimeGatedEventType.Daily) break;
                    DoublePills(award);
                    break;

                case Perk.CONVERT_PRIMOS_INTO_PILLS_ALL_1TO2_NO_MORA:
                    award.CurrenciesToAdd.Mora = 0;
                    ConvertPrimosToPills(award, 2);
                    break;

                case Perk.CONVERT_PRIMOS_INTO_PILLS_ALL_1TO2:
                    ConvertPrimosToPills(award, 2);
                    break;

                case Perk.CONVERT_MORA_INTO_PRIMOGEMS_ADVENTURE_1TO2:
                    ConvertMoraToPrimos(award, 2);
                    break;

                case Perk.CONVERT_MORA_INTO_PRIMOGEMS_ALL_1TO1:
                    ConvertMoraToPrimos(award, 1);
                    break;

                case Perk.CONVERT_PRIMOS_INTO_MORA_ALL_1TO1:
                    ConvertPrimosToMora(award, 1);
                    break;
            }
        }

        public static void DoubleMora(Award award) => award.CurrenciesToAdd.Mora *= 2;
        public static void DoublePrimos(Award award) => award.CurrenciesToAdd.Primogems *= 2;
        public static void DoublePills(Award award) => award.CurrenciesToAdd.Pills *= 2;

        public static void DoubleAward(Award award)
        {
            DoubleMora(award);
            DoublePrimos(award);
            DoublePills(award);
        }

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

        public static void ConvertMoraToPrimos(Award award, int ratio = 2)
        {
            if (award.CurrenciesToAdd.Mora < ratio) return;

            int converted = (int)(award.CurrenciesToAdd.Mora / ratio);
            award.CurrenciesToAdd.Primogems += converted;
            award.CurrenciesToAdd.Mora = 0;
        }

        public static void ConvertPrimosToMora(Award award, int ratio = 2)
        {
            if (award.CurrenciesToAdd.Primogems < ratio) return;

            int converted = (int)(award.CurrenciesToAdd.Primogems / ratio);
            award.CurrenciesToAdd.Mora += converted;
            award.CurrenciesToAdd.Primogems = 0;
        }

        public static void ConvertPrimosToPills(Award award, int ratio = 2)
        {
            if (award.CurrenciesToAdd.Primogems < ratio) return;

            int converted = (int)(award.CurrenciesToAdd.Primogems / ratio);
            award.CurrenciesToAdd.Pills += converted;
            award.CurrenciesToAdd.Primogems = 0;
        }
    }
}
