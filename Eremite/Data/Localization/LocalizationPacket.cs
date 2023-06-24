
namespace Eremite.Data.Localization
{
    [Serializable]
    public class LocalizationPacket
    {
        public List<LocalizedText> english = new List<LocalizedText>();
        public List<LocalizedText> french = new List<LocalizedText>();
        public List<LocalizedText> ukrainian = new List<LocalizedText>();
    }
}
