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

        public DiscordEmbedBuilder GetMainAkashaEmbed(List<Character> characters, Character current)
        {
            var info = new AkashaEmbedInfo(current, defaultBannerImage);

            return new DiscordEmbedBuilder()
            {
                Color = DiscordColor.Orange,
                Title = $"{user.Username} {Localization.GetText(profileKey)}",
                ImageUrl = info.profileImageUrl,
                Description = $"[ID:{user.UserId}]\n\n> **{Localization.GetText(mainCharacterKey)} {info.characterName}**" +
                    $"\n> {Localization.GetText(characterBuffKey)} {info.characterBuffInfo}\n\n{Localization.GetText(charactersObtained)} {characters.ToCharacterList()}" +
                    $"\n\n`{user.Wallet.Primogems}` {Localization.GetText(Localization.PrimosKey)} | `{user.Wallet.Mora}` {Localization.GetText(Localization.MoraKey)} | `{user.Wallet.Pills}` {Localization.GetText(Localization.PillsKey)}"
            };
        }

        private class AkashaEmbedInfo
        {
            public string characterName;
            public string characterBuffInfo;
            public string profileImageUrl;

            public AkashaEmbedInfo(Character character, string defaultImage)
            {
                var noText = Localization.GetText(SetCharacterAction.noMainCharacter);
                profileImageUrl = defaultImage;
                characterBuffInfo = noText;
                characterName = noText;

                if (character == null) return;

                characterName = character.CharacterName;
                characterBuffInfo = character.PerkInfo;
                profileImageUrl = character.ImageAkashaBannerUrl;
            }
        }
    }
}
