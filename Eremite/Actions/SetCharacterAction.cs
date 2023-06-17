using DSharpPlus.Entities;
using Eremite.Data.DiscordData;

namespace Eremite.Actions
{
    public class SetCharacterAction
    {
        public static void Equip(UserData user, Character characterToEquip)
        {
            var ownedCharacter = user.Characters.Find(character => character.CharacterName.ToLower() == characterToEquip.CharacterName.ToLower());
            if (ownedCharacter == null) return;

            user.EquippedCharacter = ownedCharacter;
        }

        public static void Dequip(UserData user) => user.EquippedCharacter = null;

        public static DiscordEmbedBuilder GetEmbedWithCharacterInfo(Character character)
        {
            var characterRarityColor = character.GetCorrespondingColor();
            string sacrificePrice = character.SellPrice > 0 ? $"\n\n> Can be sacrificed (!sacrifice) for {character.SellPrice} 💊" : string.Empty;
            return new DiscordEmbedBuilder()
            {
                Color = characterRarityColor,
                Title = character.CharacterName,
                ImageUrl = character.ImageAkashaBannerUrl,
                Description = $" {character.PerkInfo}{sacrificePrice}"
            };
        }
    }
}
