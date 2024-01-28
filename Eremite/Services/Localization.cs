using Eremite.Data;
using Eremite.Data.DiscordData;
using Eremite.Data.Localization;
using Newtonsoft.Json;

namespace Eremite.Services
{
    public class Localization
    {
        private const string LocalizationPacketName = "local.json";

        public const string MoraEmoji = "<:mora2:1122373137443598417>";
        public const string PrimosEmoji = "<:imf2pbtw:1113103136991756328>";
        public const string PillsEmoji = "<:pillwhite:1119700330259693629>";
        public const string StarEmoji = "<:Stardust3:1122370119608303716>";
        public const string MelusineEmoji = "<:melusine_sticker:1200997903901134938>";
        public const string WelkinEmoji = "<:welkin:1201068245940174858>";

        public const string Mora = "currency.mora";
        public const string Primos = "currency.primogems";
        public const string Pills = "currency.pills";

        public const string NoCurrencyKey = "not_enough_currency";

        protected static LocalizationPacket localizationPacket = new LocalizationPacket();

        internal async Task InitPacketAsync() => await LoadJson();

        /// <summary>
        /// Gets Text based on the <see cref="Language"/> provided
        /// </summary>
        /// <param name="key">Key to search</param>
        /// <returns>language-ready string</returns>
        public static string GetText(Language lang, string key) => localizationPacket.GetText(lang, key);

        /// <summary>
        /// Gets Text based on the user chosen <see cref="Language"/>
        /// </summary>
        /// <param name="key">Key to search</param>
        /// <returns>language-ready string</returns>
        public static string GetText(UserData user, string key) => localizationPacket.GetText(user.Stats.Language, key);

        public async Task LoadJson()
        {
            var jsonRawData = await DataGrabber.GrabFromConfigs(LocalizationPacketName);

            jsonRawData.LogStatus(LocalizationPacketName);

            localizationPacket = JsonConvert.DeserializeObject<LocalizationPacket>(jsonRawData);
        }

        public static void ChangeLanugage(UserData user, Language languageToSet)
        {
            if (user.Stats.Language == languageToSet) return;

            user.Stats.Language = languageToSet;
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
