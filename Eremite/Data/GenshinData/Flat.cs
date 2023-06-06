namespace Eremite.Data.GenshinData
{
    [Serializable]
    public class Flat
    {
        public string nameTextHashMap;
        public string setNameTextHashMap;
        public int rankLevel;
        public Stat reliquaryMainstat;
        public List<Stat> reliquarySubstats = new List<Stat>();
        public List<Stat> weaponStats = new List<Stat>();
        public ItemType itemType;
        public EquipType equipType;
        public string icon;
    }
}