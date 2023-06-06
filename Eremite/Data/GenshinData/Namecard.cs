
namespace Eremite.Data.GenshinData
{
    [Serializable]
    public class Namecard
    {
        public long namecardId;
        public NamecardSettings settings;

        public Namecard(int namecardId, NamecardSettings settings)
        {
            this.namecardId = namecardId;
            this.settings = settings;
        }
    }
}
