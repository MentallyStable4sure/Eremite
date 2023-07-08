using Eremite.Data.DiscordData;
using Eremite.Data.Localization;
using Eremite.Services;

namespace Eremite
{
    public static class LocalizationExtensions
    {
        public static void Add(this LocalizationPacket packet, Language language, string key, string text)
        {
            var localizedText = new LocalizedText() { key = key, text = text };

            packet.Add(language, localizedText);
        }

        public static void Add(this LocalizationPacket packet, Language language, LocalizedText text)
        {
            switch (language)
            {
                case Language.French:
                    packet.french.Add(text);
                    break;

                case Language.Ukrainian:
                    packet.ukrainian.Add(text);
                    break;

                case Language.Russian:
                    packet.russian.Add(text);
                    break;

                default:
                    packet.english.Add(text);
                    break;
            }
        }

        public static string GetText(this LocalizationPacket packet, Language language, string key)
        {
            string text = null;
            switch (language)
            {
                case Language.French:
                    text = packet.french.Find(match => match.key == key)?.text;
                    break;

                case Language.Ukrainian:
                    text = packet.ukrainian.Find(match => match.key == key)?.text;
                    break;

                case Language.Russian:
                    text = packet.russian.Find(match => match.key == key)?.text;
                    break;

                default:
                    text = packet.english.Find(match => match.key == key)?.text;
                    break;
            }

            if (text == null) return "[ERROR] No language key found";
            return text;
        }

        public static void Remove(this LocalizationPacket packet, Language language, string key)
        {
            var list = new List<LocalizedText>();
            switch (language)
            {
                case Language.French:
                    list = packet.french;
                    break;

                case Language.Ukrainian:
                    list = packet.ukrainian;
                    break;

                case Language.Russian:
                    list = packet.russian;
                    break;

                default:
                    list = packet.english;
                    break;

            }

            for (int i = 0; i < list.Count; i++)
            {
                if (list[i].key != key) continue;

                packet.french.Remove(list[i]);
                packet.ukrainian.Remove(list[i]);
                packet.english.Remove(list[i]);
                packet.russian.Remove(list[i]);
            }

        }

        public static string GetText(this UserData user, string key) => Localization.GetText(user, key);

    }
}
