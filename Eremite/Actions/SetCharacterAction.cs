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

        public static DiscordEmbedBuilder GetEmbedWithEquippedCharacter(Character equippedCharacter)
        {
            var characterRarityColor = equippedCharacter.GetCorrespondingColor();
            return new DiscordEmbedBuilder()
            {
                Color = characterRarityColor,
                Title = equippedCharacter.CharacterName,
                ImageUrl = equippedCharacter.ImageAkashaBannerUrl,
                Description = $"> {equippedCharacter.PerkInfo}"
            };
        }
    }
}
