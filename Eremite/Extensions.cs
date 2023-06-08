using DSharpPlus.Entities;
using Eremite.Commands;
using Eremite.Data.DiscordData;

namespace Eremite
{
    public static class Extensions
    {

        public static bool IsValid(this UserData user) => user.UserId != null && user.UserId != string.Empty;

        public static void LogStatus(this string rawJson, string fileName = "")
        {
            bool isCorrupted = rawJson == null || rawJson.Length <= 0;

            string corruptedMessage = $"[ERROR] Couldnt load {fileName}";
            string successMessage = $"[SUCCESS] {fileName} loaded successfully";

            Console.WriteLine(isCorrupted ? corruptedMessage : successMessage);
        }

        public static string ToCharacterList(this List<Character> characters)
        {
            if (characters.Count <= 0) return AkashaCommand.DefaultNullError;

            string charactersInInventory = string.Empty;
            foreach (var character in characters)
            {
                charactersInInventory = $"{charactersInInventory} {character.CharacterName} <{character.StarsRarity}{AkashaCommand.StarSign}> ";
            }

            return charactersInInventory;
        }

        public static Character GetHighestTier(this List<Character> characters)
        {
            var highestTier = characters[0];
            foreach (var character in characters)
            {
                if (character.StarsRarity < highestTier.StarsRarity) continue;
                highestTier = character;
            }

            return highestTier;
        }

        public static DiscordColor GetCorrespondingColor(this Character character)
        {
            switch (character.StarsRarity)
            {
                case 3: return DiscordColor.Green;
                case 4: return DiscordColor.Blue;
                case 5: return DiscordColor.Orange;
                case 10: return DiscordColor.Red;
                default: return DiscordColor.White;
            }
        }
    }
}
