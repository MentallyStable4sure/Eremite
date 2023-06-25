using Eremite.Data;
using Eremite.Data.Localization;
using Newtonsoft.Json;

namespace Eremite.Services
{
    public class Localization
    {
        private const string LocalizationPacketName = "local.json";

        public const string MoraKey = "mora";
        public const string PrimosKey = "primos";
        public const string PillsKey = "pills";
        public const string StarKey = "star";

        public const string NoCurrencyKey = "not_enough_currency";

        protected static LocalizationPacket localizationPacket = new LocalizationPacket();

        public static Language CurrentLanguage { get; protected set; } = Language.English;

        public event Action<Language> OnLanguageChanged;

        internal async Task InitPacketAsync() => await LoadJson();

        /// <summary>
        /// Gets Text based on current <see cref="Language"/>
        /// </summary>
        /// <param name="key">Key to search</param>
        /// <returns>language-ready string</returns>
        public static string GetText(string key) => localizationPacket.GetText(CurrentLanguage, key);

        public async Task LoadJson()
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
