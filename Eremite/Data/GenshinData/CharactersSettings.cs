namespace Eremite.Data.GenshinData
{
    [Serializable]
    public class CharactersSettings
    {
        public string Element;
        public List<string> Consts = new List<string>();
        public List<long> SkillOrder = new List<long>();

        public Dictionary<string, string> Skills = new Dictionary<string, string>();
        public Dictionary<string, long> ProudMap = new Dictionary<string, long>();
        public long NameTextMapHash;

        public string SideIconName;
        public string QualityType;
    }
}