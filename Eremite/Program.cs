using DSharpPlus;
using Newtonsoft.Json;
using DSharpPlus.Entities;
using Eremite.Data;

namespace Eremite
{
    internal class Program
    {
        public const string SumeruVibes = "sumeru_vibes.json";

        static async Task Main(string[] args)
        {
            var rawConfig = await DataHandler.ReadFromConfigs(DataHandler.StartupConfig);
            var config = JsonConvert.DeserializeObject<Config>(rawConfig);

            var discord = new DiscordClient(config?.CreateDiscordConfig());
            DiscordActivity activity = await SetStatus();

            await discord.ConnectAsync(activity, UserStatus.Idle);
            await Task.Delay(-1);
        }

        private static async Task<DiscordActivity> SetStatus()
        {
            return new DiscordActivity()
            {
                ActivityType = ActivityType.ListeningTo,
                Name = await GetVibes()
            };
        }

        private static async Task<string> GetVibes()
        {
            var currentVibe = "Sand Dunes";
            var rawVibes = await DataHandler.ReadFromConfigs(SumeruVibes);

            var sumeruVibes = JsonConvert.DeserializeObject<List<string>>(rawVibes);
            if (sumeruVibes == null) return currentVibe;

            currentVibe = sumeruVibes[Random.Shared.Next(sumeruVibes.Count)];
            return currentVibe;
        }
    }
}