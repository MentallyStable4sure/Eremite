
namespace Eremite.Data
{
    internal class DataGrabber
    {
        public const string ContentFolder = "content";
        public const string ConfigsFolder = "configs";

        public static async Task<string> GrabFromConfigs(string configFile) => await GrabFromFile(ConfigsFolder, configFile);

        public static async Task<string> GrabFromContent(string contentFile) => await GrabFromFile(ContentFolder, contentFile);

        public static async Task<string> GrabFromFile(string directory, string file)
        {
            var current = Directory.GetCurrentDirectory();
            var fullPath = Path.Combine(current, directory);

            if (!Directory.Exists(fullPath)) return string.Empty;
            fullPath = Path.Combine(fullPath, file);

            return await File.ReadAllTextAsync(fullPath);
        }

        public static FileStream GrabFromContentStream(string contentFile)
        {
            var current = Directory.GetCurrentDirectory();
            var fullPath = Path.Combine(current, ContentFolder);

            if (!Directory.Exists(fullPath)) return (FileStream)FileStream.Null;

            return File.OpenRead(fullPath);
        }
    }
}
