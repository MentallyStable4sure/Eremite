
namespace Eremite.Data.GenshinData
{
    [Serializable]
    public class GenshinUserData
    {
        public PlayerInfo playerInfo;
        public List<AvatarInfoList> avatarInfoList = new List<AvatarInfoList>();
        public long ttl;
    }
}
