using Eremite.Services;
using Eremite.PerkActions;
using Eremite.Base.Interfaces;
using Eremite.Data.DiscordData;

namespace Eremite.Actions
{
    //TODO: DIVIDE PERKS INTO SMALLER PIECES AS CLASSES AND INTERFACES
    public class PerkHandler : IEremiteService
    {
        public DataHandler DataHandler { get; set; }

        public readonly IPerkAction[] PerkActions;

        public PerkHandler(DataHandler dataHandler)
        {
            DataHandler = dataHandler;

            PerkActions = new IPerkAction[]
            {
                new DoubleMoraLowerPrimos(),
                new DoubleMora(),
                new DoublePrimosLowerMora(),
                new DoublePrimos(),
                new TwiceAdventureBounty(),
                new LowerAdventureCdTeamDependent(),
                new LowerDailyCdTeamDependent(),
                new LowerAdventureCdPermanent(),
                new LowerDailyCdPermanent(),
                new ConvertMoraIntoPrimosAdventure1To2(),
                new ConvertMoraIntoPrimosAll1To1(),
                new ConvertPrimosIntoMoraAll1To1(),
                new DoublePillsDaily(),
                new ConvertPrimosIntoPillsAll1To2NoMora(),
                new ConvertPrimosIntoPillsAll1To2(),
                new OnSacrificePrimos160PerMelusineHelped(),
                new OnSacrificePrimos3600OrRandomChar5050Chance(),
                new OnSacrifice10PillPerChar(),
                new OnSacrifice10kMoraPerChar(),
                new OnSacrificeRefreshWelkinCooldown(),
                new FishblastingFishPerSacrificableChar(),
                new Minus1kMoraPerAction(),
                new FishblastingRareReward5050(),
                new FishblastingRareRewards(),
                new Adventurex3RewardOnMelusineFound(),
                new FishblastingRandomCharacter()
            };
        }

        public string ApplyPerk(UserData user, TimeGatedEvent timeGatedEvent, Award award)
        {
            if (!user.IsAnyCharacterEquipped()) return string.Empty;
            var eventType = timeGatedEvent.EventType;
            var perk = (Perk)(CharactersHandler.ConvertId(user.EquippedCharacter).PerkStat);

            Console.WriteLine($"Applying perk from user {user.Username}, event type: {eventType.ToString()}, equipped char: {CharactersHandler.ConvertId(user.EquippedCharacter).CharacterName}, perk: {perk}");

            string additionInfo = string.Empty;

            foreach (var action in PerkActions)
            {
                if(perk != action.PerkNeededToProc) continue;
                additionInfo = action.DoAction(user, DataHandler, timeGatedEvent, award);
            }

            RemoveCharacterIfNeeded(user);
            return additionInfo;
        }

        private void RemoveCharacterIfNeeded(UserData user)
        {
            var equippedChar = CharactersHandler.ConvertId(user.EquippedCharacter);
            if (equippedChar == null) return;
            if (equippedChar.ShouldBeDestroyed == false) return;

            SetCharacterAction.Dequip(user);
            user.RemovePulledCharacter(equippedChar);
        }
    }
}
