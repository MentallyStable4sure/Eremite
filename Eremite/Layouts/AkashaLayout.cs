using DSharpPlus.Entities;
using Eremite.Services;
using Eremite.Data.DiscordData;
using Eremite.Actions;

namespace Eremite.Layouts
{
    internal class AkashaLayout
    {
        private UserData user;
        private string defaultBannerImage;

        public const string profileKey = "profile.user_profile";
        public const string mainCharacterKey = "profile.main_character";
        public const string characterBuffKey = "profile.character_buff";
        public const string charactersObtained = "profile.characters_obtained";

        public AkashaLayout(UserData user, string defaultBannerImage)
        {
            this.defaultBannerImage = defaultBannerImage;
            this.user = user;
        }

        public DiscordEmbedBuilder GetMainAkashaEmbed(UserData user, List<Character> characters, Character current)
        {
            var info = new AkashaEmbedInfo(user, current, defaultBannerImage);
            var equippedInfo = user.Stats.EquippedItem == null ? string.Empty : $"Equipped: {user.Stats.EquippedItem.EmojiCode}";
            return new DiscordEmbedBuilder()
            {
                Color = DiscordColor.Orange,
                Title = $"{user.Username} {user.GetText(profileKey)}",
                ImageUrl = info.profileImageUrl,
                Description = $"[ID:{user.UserId}]\n\n> **{user.GetText(mainCharacterKey)} {info.characterName}**" +
                    $"\n> {user.GetText(characterBuffKey)} {info.characterBuffInfo}\n\n{user.GetText(charactersObtained)} {characters.ToCharacterList(user)}" +
                    $"\n\n`{user.Wallet.Primogems}` {Localization.PrimosEmoji} | `{user.Wallet.Mora}` {Localization.MoraEmoji} | `{user.Wallet.Pills}` {Localization.PillsEmoji} | \n>{equippedInfo}"
            };
        }

        private class AkashaEmbedInfo
        {
            public string characterName;
            public string characterBuffInfo;
            public string profileImageUrl;

            public AkashaEmbedInfo(UserData user, Character character, string defaultImage)
            {
                var noText = user.GetText(SetCharacterAction.noMainCharacter);
                profileImageUrl = defaultImage;
                characterBuffInfo = noText;
                characterName = noText;

                if (character == null) return;

                characterName = user.GetText($"character.{character.CharacterId}.name");
                characterBuffInfo = user.GetText($"character.{character.CharacterId}.perk_info");
                profileImageUrl = character.ImageAkashaBannerUrl;
            }
        }
    }
}
