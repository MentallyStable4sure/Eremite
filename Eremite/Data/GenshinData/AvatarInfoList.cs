
namespace Eremite.Data.GenshinData
{
    [Serializable]
    public class AvatarInfoList
    {
        public int avatarID;
        public List<int> talentIdList = new List<int>();
        public List<int> inherentProudSkillList = new List<int>();
        public int skillDepotId;
        public PropMap propMap;
        public List<EquipList> equipList = new List<EquipList>();
    }
}