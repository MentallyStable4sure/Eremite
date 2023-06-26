using DSharpPlus.Entities;
using Eremite.Data.DiscordData;
using Eremite.Services;

namespace Eremite.Actions
{
    public class SetCharacterAction
    {
        public const string sacrificableCharacter = "setcharacter.char_can_be_sacrificed";
        public const string noMainCharacter = "setcharacter.no_main_character";

        public static void Equip(UserData user, Character characterToEquip)
        {
            var ownedCharacter = user.Characters.Find(character => character == characterToEquip.CharacterId);
            if (!ownedCharacter.IsCharacterValid()) return;

            user.EquippedCharacter = ownedCharacter;
        }

        public static void Dequip(UserData user) => user.EquippedCharacter = UserExtensions.UnsetId;

        public static DiscordEmbedBuilder GetEmbedWithCharacterInfo(UserData user, Character character)
        {
            var characterRarityColor = character.GetCorrespondingColor();
            string sacrificePrice = character.SellPrice > 0 ? $"\n\n> {user.GetText(sacrificableCharacter)} [{character.SellPrice} {Localization.PillsEmoji}]" : string.Empty;
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
