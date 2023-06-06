﻿
namespace Eremite.Data.DiscordData
{
    [Serializable]
    public class Character
    {
        public string CharacterName = string.Empty;
        public int StarsRarity = 3;
        public string ImageAkashaBannerUrl = string.Empty;
        public string ImagePullBannerUrl = string.Empty;
        public int PerkStat;
        public string PerkInfo = string.Empty;
        public bool ShouldBeDestroyed = false;
        public bool ShouldBeDestroyedOnEnroll = false;
    }
}
