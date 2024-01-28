using Eremite.Services;
using Eremite.Data.DiscordData;

namespace Eremite.PerkActions
{
    public class OnSacrificePrimos3600OrRandomChar5050Chance : BasePerkAction
    {
        public OnSacrificePrimos3600OrRandomChar5050Chance()
        {
            PerkNeededToProc = Perk.ON_SACRIFICE_RANDOM_CHARACTER_OR_3600_PRIMOS_50_50;

            EventsWhereCanBeProced = new TimeGatedEventType[] { TimeGatedEventType.Sacrifice };
        }

        protected override string OnProced(UserData user, DataHandler data, TimeGatedEvent eventProced, Award award)
        {
            bool isLucky = Random.Shared.Next(0, 101) <= 50;
            if (isLucky)
            {
                award.CurrenciesToAdd.Primogems += 3600;
                additionalInfo = $"> ➕3600{Localization.PrimosEmoji}";
                return additionalInfo;
            }

            var star = data.Config.Chances.GetStarByChance();
            var charactersPool = CharactersHandler.CharactersData.GetCharactersPoolByStar(star);

            var randomCharacter = charactersPool[Random.Shared.Next(0, charactersPool.Count)];
            award.CharactersToAdd.Add(randomCharacter);
            additionalInfo = $"> 🆕 {award.CharactersToAdd.ToCharacterList(user)}";
            return additionalInfo;
        }
    }
}
