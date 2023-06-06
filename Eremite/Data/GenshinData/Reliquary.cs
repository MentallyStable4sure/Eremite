
namespace Eremite.Data.GenshinData
{
    [Serializable]
    public class Reliquary
    {
        public int level;
        public int mainPropId;
        public List<int> appendPropIdList = new List<int>();
    }
}