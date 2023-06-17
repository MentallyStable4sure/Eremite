using Eremite.Data.DiscordData;

namespace Eremite
{
    public static class UserExtensions
    {
        public static bool IsValid(this UserData user) => user.UserId != null && user.UserId != string.Empty;

        public static void AddPulledCharacter(this UserData user, Character character)
        {
            var characters = user.Characters;
            Character duplicate;

            if (characters == null) characters = new List<Character>();
            if (characters.Count <= 0)
            {
                characters.Add(character);
                return;
            }

            duplicate = characters.Find(characterSaved => characterSaved.CharacterName == character.CharacterName);

            if (duplicate != null && duplicate.StarsRarity >= 10) user.Characters.Add(duplicate);
            if (duplicate != null) return;

            user.Characters.Add(character);
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
    }
}
