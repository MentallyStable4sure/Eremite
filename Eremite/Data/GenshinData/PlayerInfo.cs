
using DiscordBot.GenshinData;

namespace Eremite.Data.GenshinData
{
    [Serializable]
    public class PlayerInfo
    {
        public string nickname;
        public string level;
        public string signature;

        public int worldLevel;
        public int nameCardId;
        public int finishAchievementNum;

        public int towerFloorIndex;
        public int towerLevelIndex;

        public List<int> showNameCardIdList = new List<int>();
        public List<ShowAvatarInfoList> showAvatarInfoList = new List<ShowAvatarInfoList>();
        public ProfilePicture profilePicture;
    }
}