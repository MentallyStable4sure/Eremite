
namespace Eremite.Data.GenshinData
{
    [Serializable]
    public class Weapon
    {
        public int level;
        public int promoteLevel;
        public Dictionary<int, int> affixMap = new Dictionary<int, int>();
    }
}