using Eremite.Data.DiscordData;

namespace Eremite
{
    public static class UserExtensions
    {
        public const int UnsetId = 0;

        public static bool IsValid(this UserData user) => user.UserId != null && user.UserId != string.Empty;

        public static void AddPulledCharacter(this UserData user, Character character) => AddPulledCharacter(user, character.CharacterId);

        public static void AddPulledCharacter(this UserData user, int characterId)
        {
            if(user.Characters == null || user.Characters.Count <= 0)
            {
                user.Characters = new List<int>() { characterId };
                return;
            }

            if(user.Characters.Contains(characterId)) return;
            user.Characters.Add(characterId);
        }

        public static void RemovePulledCharacter(this UserData user, Character character) => RemovePulledCharacter(user, character.CharacterId);

        public static void RemovePulledCharacter(this UserData user, int characterId)
        {
            if (user.EquippedCharacter == characterId) user.EquippedCharacter = 0;
            user.Characters.Remove(characterId);
        }

        public static void ResetWallet(this UserData user) => user.Wallet = new DiscordWallet();

        public static void ResetStats(this UserData user) => user.Stats = new Stats();

        public static void AddCurrency(this UserData user, DiscordWallet wallet)
        {
            user.Wallet.Primogems += wallet.Primogems;
            user.Wallet.Mora += wallet.Mora;
            user.Wallet.Pills += wallet.Pills;

            user.Stats.TotalPrimogemsEarned += wallet.Primogems;
            user.Stats.TotalPillsEarned += wallet.Pills;
        }

        public static void AddAward(this UserData user, Award award)
        {
            foreach (var character in award.CharactersToAdd)
            {
                user.AddPulledCharacter(character);
            }

            user.AddCurrency(award.CurrenciesToAdd);
        }

        public static bool IsAnyCharacterEquipped(this UserData user) => IsCharacterValid(user.EquippedCharacter);

        public static bool IsCharacterValid(this int id) => id != UnsetId;
    }
}
