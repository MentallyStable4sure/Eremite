using Eremite.Services;
using Eremite.Data.DiscordData;
using Eremite.Base.Interfaces;

namespace Eremite.Actions
{
    //TODO: DIVIDE PERKS INTO SMALLER PIECES AS CLASSES AND INTERFACES
    public class PerkAction : IEremiteService
    {
        public DataHandler DataHandler { get; set; }

        public PerkAction(DataHandler dataHandler) => DataHandler = dataHandler;

        public const int MinutesCooldownPerCharacterAdventure = 10;
        public const int MinutesCooldownPerCharacterDaily = 30;
        public const int ChancesPerCharacterResetCDAdventure = 5;

        public const float PercentageMaxReset = 0.75f; //75%

        public const int HoursCooldownAdventurePermanent = 1;
        public const int HoursCooldownDailyPermanent = 12;

        public string ApplyPerk(UserData user, TimeGatedEventType eventType, Award award)
        {
            if (!user.IsAnyCharacterEquipped()) return string.Empty;
            var perk = (Perk)(CharactersHandler.ConvertId(user.EquippedCharacter).PerkStat);
            Console.WriteLine($"Applying perk from user {user.Username}, event type: {eventType.ToString()}, equipped char: {user.EquippedCharacter}, perk: {perk}");

            int baseAmount = 0;
            int convertion = 0;
            string additionalInfo = string.Empty;

            //TODO: DIVIDE PERKS INTO SMALLER PIECES AS CLASSES AND INTERFACES WTF IS THAT WORM OF CASES BRO
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
                    baseAmount = award.CurrenciesToAdd.Mora;
                    DoublePrimos(award);
                    award.CurrenciesToAdd.Mora = award.CurrenciesToAdd.Mora > 0 ? award.CurrenciesToAdd.Mora / 2 : 0;
                    additionalInfo = $"> {baseAmount}{Localization.MoraEmoji} x2 => {award.CurrenciesToAdd.Mora}{Localization.MoraEmoji}";
                    break;

                case Perk.DOUBLE_PRIMOS:
                    baseAmount = award.CurrenciesToAdd.Primogems;
                    DoublePrimos(award);
                    additionalInfo = $"> {baseAmount}{Localization.PrimosEmoji} x2 => {award.CurrenciesToAdd.Primogems}{Localization.PrimosEmoji}";
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
                    baseAmount = award.CurrenciesToAdd.Pills;
                    DoublePills(award);
                    additionalInfo = $"> {baseAmount}{Localization.PillsEmoji} x2 => {award.CurrenciesToAdd.Pills}{Localization.PillsEmoji}";
                    break;

                case Perk.CONVERT_PRIMOS_INTO_PILLS_ALL_1TO2_NO_MORA:
                    baseAmount = award.CurrenciesToAdd.Primogems;
                    award.CurrenciesToAdd.Mora = 0;
                    convertion = ConvertPrimosToPills(award, 2);
                    additionalInfo = $"> {baseAmount}{Localization.PrimosEmoji} => {convertion}{Localization.PillsEmoji}";
                    break;

                case Perk.CONVERT_PRIMOS_INTO_PILLS_ALL_1TO2:
                    baseAmount = award.CurrenciesToAdd.Primogems;
                    convertion = ConvertPrimosToPills(award, 2);
                    additionalInfo = $"> {baseAmount}{Localization.PrimosEmoji} => {convertion}{Localization.PillsEmoji}";
                    break;

                case Perk.CONVERT_MORA_INTO_PRIMOGEMS_ADVENTURE_1TO2:
                    baseAmount = award.CurrenciesToAdd.Mora;
                    convertion = ConvertMoraToPrimos(award, 2);
                    additionalInfo = $"> {baseAmount}{Localization.MoraEmoji} => {convertion}{Localization.PrimosEmoji}";
                    break;

                case Perk.CONVERT_MORA_INTO_PRIMOGEMS_ALL_1TO1:
                    baseAmount = award.CurrenciesToAdd.Mora;
                    convertion = ConvertMoraToPrimos(award, 1);
                    additionalInfo = $"> {baseAmount}{Localization.MoraEmoji} => {convertion}{Localization.PrimosEmoji}";
                    break;

                case Perk.CONVERT_PRIMOS_INTO_MORA_ALL_1TO1:
                    baseAmount = award.CurrenciesToAdd.Primogems;
                    convertion = ConvertPrimosToMora(award, 1);
                    additionalInfo = $"> {baseAmount}{Localization.PrimosEmoji} => {convertion}{Localization.MoraEmoji}";
                    break;

                case Perk.ON_SACRIFICE_GIVES_10_PILLS_PER_CHAR_IN_INVENTORY:
                    if (eventType != TimeGatedEventType.Sacrifice) break;
                    GiveAwardPerChar(user, award, new Award(new DiscordWallet(0, 0, 10)));
                    additionalInfo = $"> ➕{award.CurrenciesToAdd.Pills * user.Characters.Count}{Localization.PillsEmoji}";
                    break;

                case Perk.ON_SACRIFICE_PRIMOS_1600_PER_MELUSINE:
                    if (eventType != TimeGatedEventType.Sacrifice) break;
                    int amountToAdd = user.Stats.MelusinesHelped <= 0 ? 1600 : 1600 * user.Stats.MelusinesHelped;
                    award.CurrenciesToAdd.Primogems += amountToAdd;
                    additionalInfo = $"> ➕{amountToAdd}{Localization.PrimosEmoji}";
                    break;

                case Perk.ON_SACRIFICE_RANDOM_CHARACTER_OR_3600_PRIMOS_50_50:
                    if (eventType != TimeGatedEventType.Sacrifice) break;
                    award.CurrenciesToAdd.Primogems += 3600;

                    var star = DataHandler.Config.Chances.GetStarByChance();
                    var charactersPool = CharactersHandler.CharactersData.GetCharactersPoolByStar(star);

                    var randomCharacter = charactersPool[Random.Shared.Next(0, charactersPool.Count)];
                    award.CharactersToAdd.Add(randomCharacter);
                    additionalInfo = $"> 🆕 {award.CharactersToAdd.ToCharacterList(user)}";
                    break;

                case Perk.WHEN_SACRIFICED_REFRESHES_WELKIN_COOLDOWN:
                    if (eventType != TimeGatedEventType.Sacrifice) break;

                    foreach (var userEvent in user.Events)
                    {
                        if (userEvent.EventType != TimeGatedEventType.Welkin) continue;

                        userEvent.TimeBetweenTriggers = TimeSpan.FromSeconds(1);
                    }
                    break;

                case Perk.ON_SACRIFICE_GIVES_10000_MORA_PER_CHAR_IN_INVENTORY:
                    if (eventType != TimeGatedEventType.Sacrifice) break;
                    GiveAwardPerChar(user, award, new Award(new DiscordWallet(0, 10000)));
                    break;

                case Perk.MINUS1000MORA_PER_ACTION:
                    if (user.Wallet.Mora < 1000) break;
                    GiveAwardPerChar(user, award, new Award(new DiscordWallet(0, -1000)));
                    break;

                default:
                    break;
            }

            RemoveCharacterIfNeeded(user);
            return additionalInfo;
        }

        private void RemoveCharacterIfNeeded(UserData user)
        {
            var equippedChar = CharactersHandler.ConvertId(user.EquippedCharacter);
            if (equippedChar == null) return;
            if (equippedChar.ShouldBeDestroyed == false) return;

            SetCharacterAction.Dequip(user);
            user.RemovePulledCharacter(equippedChar);
        }

        private static void GiveAwardPerChar(UserData user, Award award, Award rewardPerCharacter)
        {
            var charsCount = user.Characters.Count;

            award.CurrenciesToAdd.Mora += (charsCount * rewardPerCharacter.CurrenciesToAdd.Mora);
            award.CurrenciesToAdd.Primogems += (charsCount * rewardPerCharacter.CurrenciesToAdd.Primogems);
            award.CurrenciesToAdd.Pills += (charsCount * rewardPerCharacter.CurrenciesToAdd.Pills);
        }

        private static void MultiplyAwardPerChar(UserData user, Award award, int multiplier)
        {
            var charsCount = user.Characters.Count;

            award.CurrenciesToAdd.Mora = (award.CurrenciesToAdd.Mora * multiplier) * charsCount;
            award.CurrenciesToAdd.Primogems *= (award.CurrenciesToAdd.Primogems * multiplier) * charsCount;
            award.CurrenciesToAdd.Pills *= (award.CurrenciesToAdd.Pills * multiplier) * charsCount;
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

        public static int ConvertMoraToPrimos(Award award, int ratio = 2)
        {
            if (award.CurrenciesToAdd.Mora < ratio) return 0;

            int converted = (int)(award.CurrenciesToAdd.Mora / ratio);
            award.CurrenciesToAdd.Primogems += converted;
            award.CurrenciesToAdd.Mora = 0;
            
            return converted;
        }

        public static int ConvertPrimosToMora(Award award, int ratio = 2)
        {
            if (award.CurrenciesToAdd.Primogems < ratio) return 0;

            int converted = (int)(award.CurrenciesToAdd.Primogems / ratio);
            award.CurrenciesToAdd.Mora += converted;
            award.CurrenciesToAdd.Primogems = 0;

            return converted;
        }

        public static int ConvertPrimosToPills(Award award, int ratio = 2)
        {
            if (award.CurrenciesToAdd.Primogems < ratio) return 0;

            int converted = (int)(award.CurrenciesToAdd.Primogems / ratio);
            award.CurrenciesToAdd.Pills += converted;
            award.CurrenciesToAdd.Primogems = 0;

            return converted;
        }
    }
}
