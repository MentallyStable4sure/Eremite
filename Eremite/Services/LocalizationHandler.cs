using Eremite.Data;
using Eremite.Data.Localization;
using Newtonsoft.Json;

namespace Eremite.Services
{
    public class LocalizationHandler
    {
        private const string LocalizationPacketName = "local.json";

        protected LocalizationPacket localizationPacket = new LocalizationPacket();

        public Language CurrentLanguage { get; protected set; } = Language.English;

        public event Action<Language> OnLanguageChanged;

        internal async Task InitPacketAsync() => await LoadJson();

        public string GetTextBasedOnLanguage(string key) => localizationPacket.GetText(CurrentLanguage, key);

        private async Task LoadJson()
        {
            var jsonRawData = await DataGrabber.GrabFromConfigs(LocalizationPacketName);

            jsonRawData.LogStatus("Localization Packet");

            localizationPacket = JsonConvert.DeserializeObject<LocalizationPacket>(jsonRawData);
            OnLanguageChanged?.Invoke(CurrentLanguage);
        }

        public void ChangeLanugage(Language languageToSet)
        {
            if (CurrentLanguage == languageToSet) return;

            CurrentLanguage = languageToSet;
            OnLanguageChanged?.Invoke(CurrentLanguage);
        }

        //Generate JSON (USE TO CREATE A FIRST-TIME DUMMY)
        internal void GenerateDummyJson()
        {
            localizationPacket.Add(Language.English, "dummy_key", "Wrong login or password, try again or you might wanna register?");
            localizationPacket.Add(Language.French, "dummy_key", "dummy text about this key");
            localizationPacket.Add(Language.French, "dummy_key2", "dummy text about this key");
            localizationPacket.Add(Language.Ukrainian, "dummy_key", "dummy text about this key");
            localizationPacket.Add(Language.Ukrainian, "dummy_key2", "dummy text about this key");

            var rawJson = JsonConvert.SerializeObject(localizationPacket, Formatting.Indented);
            File.WriteAllText(Path.Combine(Directory.GetCurrentDirectory(), LocalizationPacketName), rawJson);
        }
    }
}
