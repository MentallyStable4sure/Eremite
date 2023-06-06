﻿
namespace Eremite.Data
{
    internal class DataRouter
    {
        public const string StartupConfig = "startup.json";
        public const string ContentFolder = "content";
        public const string ConfigsFolder = "configs";

        public static async Task<string> ReadFromConfigs(string configFile) => await ReadFromFile(ConfigsFolder, configFile);

        public static async Task<string> ReadFromContent(string contentFile) => await ReadFromFile(ContentFolder, contentFile);

        public static async Task<string> ReadFromFile(string directory, string file)
        {
            var current = Directory.GetCurrentDirectory();
            var fullPath = Path.Combine(current, directory);

            if (!Directory.Exists(fullPath)) return string.Empty;
            fullPath = Path.Combine(fullPath, file);

            return await File.ReadAllTextAsync(fullPath);
        }
    }
}