
using DSharpPlus;
using DSharpPlus.Entities;
using Newtonsoft.Json;
using Eremite.Data;

namespace Eremite.Services
{
    internal class BotProfileHandler 
    {
        private StartupConfig? botStartupConfig;
        private DiscordActivity currentStatus;

        public const string StartupConfig = "startup_config.json";
        public const string SumeruVibes = "sumeru_vibes.json";

        public StartupConfig GetBotConfig() => botStartupConfig;

        public async Task<DiscordConfiguration> SetConfig()
        {
            var rawConfig = await DataGrabber.GrabFromConfigs(StartupConfig);

            rawConfig.LogStatus(StartupConfig);

            botStartupConfig = JsonConvert.DeserializeObject<StartupConfig>(rawConfig);

            return CreateDiscordConfig(botStartupConfig);
        }

        public DiscordConfiguration SetConfig(StartupConfig customConfig) => CreateDiscordConfig(customConfig);

        public DiscordConfiguration CreateDiscordConfig(StartupConfig config)
        {
            return new DiscordConfiguration()
            {
                Token = config.Token,
                TokenType = config.TokenType,
                Intents = DiscordIntents.AllUnprivileged | DiscordIntents.MessageContents
            };
        }

        public async Task<DiscordActivity> SetStatus()
        {
            currentStatus = new DiscordActivity()
            {
                ActivityType = ActivityType.ListeningTo,
                Name = await GetVibes()
            };

            return currentStatus;
        }

        private async Task<string> GetVibes()
        {
            var currentVibe = "Sand Dunes";
            var rawVibes = await DataGrabber.GrabFromConfigs(SumeruVibes);

            var sumeruVibes = JsonConvert.DeserializeObject<List<string>>(rawVibes);
            if (sumeruVibes == null) return currentVibe;

            currentVibe = sumeruVibes[Random.Shared.Next(sumeruVibes.Count)];
            return currentVibe;
        }

    }
}
