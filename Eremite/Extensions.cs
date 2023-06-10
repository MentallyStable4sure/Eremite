using DSharpPlus.Entities;
using Eremite.Commands;
using Eremite.Data;
using Eremite.Data.DiscordData;
using Newtonsoft.Json;

namespace Eremite
{
    public static class Extensions
    {
        public static string GetNormalTime(this TimeSpan time) => time.ToString(@"dd\.hh\:mm\:ss");

        public static void LogStatus(this string rawJson, string fileName = "")
        {
            bool isCorrupted = rawJson == null || rawJson.Length <= 0;

            string corruptedMessage = $"[ERROR] Couldnt load {fileName}";
            string successMessage = $"[SUCCESS] {fileName} loaded successfully";

            Console.WriteLine(isCorrupted ? corruptedMessage : successMessage);
        }

        public static string ToCharacterList(this List<Character> characters)
        {
            if (characters == null || characters.Count <= 0) return AkashaCommand.DefaultNullError;

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

        public static string GetCorrespondingQuery(this QueryElement element, UserData user)
        {
            switch (element)
            {
                case QueryElement.Username: return $"`username`='{user.Username}'";
                case QueryElement.Wallet: return $"`wallet`='{JsonConvert.SerializeObject(user.Wallet)}'";
                case QueryElement.Characters: return $"`characters`='{JsonConvert.SerializeObject(user.Characters)}'";
                case QueryElement.EquippedCharacter: return $"`equippedcharacter`='{JsonConvert.SerializeObject(user.EquippedCharacter)}'";
                case QueryElement.Stats: return $"`stats`='{JsonConvert.SerializeObject(user.Stats)}'";
                case QueryElement.Events: return $"`events`='{JsonConvert.SerializeObject(user.Events)}'";
                default: return $"`userid='{user.UserId}',`username`='{user.Username}',`wallet`='{JsonConvert.SerializeObject(user.Wallet)}'," +
                        $"`characters`='{JsonConvert.SerializeObject(user.Characters)}',`equippedcharacter`='{JsonConvert.SerializeObject(user.EquippedCharacter)}'," +
                        $"`stats`='{JsonConvert.SerializeObject(user.Stats)}',`events`='{JsonConvert.SerializeObject(user.Events)}'";
            }
        }
    }
}
