using DSharpPlus.Entities;
using Eremite.Commands;
using Eremite.Data.DiscordData;

namespace Eremite.Layouts
{
    internal class AkashaLayout
    {
        private UserData _user;
        private string _defaultBannerImage;

        public AkashaLayout(UserData user, string defaultBannerImage)
        {
            _defaultBannerImage = defaultBannerImage;
            _user = user;
        }

        public DiscordEmbedBuilder GetMainAkashaEmbed( List<Character> characters, Character current)
        {
            var info = new AkashaEmbedInfo(current, _defaultBannerImage);

            return new DiscordEmbedBuilder()
            {
                Color = DiscordColor.Orange,
                Title = $"{_user.Username}'s profile",
                ImageUrl = info.profileImageUrl,
                Description = $"[ID:{_user.UserId}]\n\n> **Main Character: {info.characterName}**" +
                    $"\n> Character Buff: {info.characterBuffInfo}\n\nCharacters Obtained: {characters.ToCharacterList()}" +
                    $"\n\n`Primogems: {_user.Wallet.Primogems} | Mora: {_user.Wallet.Mora} | {_user.Wallet.Pills} 💊`"
            };
        }

        private class AkashaEmbedInfo
        {
            public string characterName = AkashaCommand.DefaultNullError;
            public string characterBuffInfo = "None, use !setcharacter [name] or !pull to get one :)";
            public string profileImageUrl;

            public AkashaEmbedInfo(Character character, string defaultImage)
            {
                profileImageUrl = defaultImage;
                if (character == null) return;

                characterName = character.CharacterName;
                characterBuffInfo = character.PerkInfo;
                profileImageUrl = character.ImageAkashaBannerUrl;
            }
        }
    }
}
