
namespace Eremite.Data.GenshinData
{
    [Serializable]
    public class NamecardSettings
    {
        public long nameTextMapHash;
        public string icon;

        public List<string> picPath = new List<string>();
        public int rankLevel;
        public string materialType;
    }
}