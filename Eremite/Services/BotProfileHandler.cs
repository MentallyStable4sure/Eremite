
using DSharpPlus;
using Newtonsoft.Json;
using DSharpPlus.Entities;
using Eremite.Data;

namespace Eremite.Services
{
    internal class BotProfileHandler 
    {
        private Config? botConfig;
        private DiscordActivity currentStatus;

        public const string SumeruVibes = "sumeru_vibes.json";

        public Config GetConfig() => botConfig;

        public async Task<DiscordConfiguration> SetConfig()
        {
            var rawConfig = await DataRouter.ReadFromConfigs(DataRouter.StartupConfig);
            botConfig = JsonConvert.DeserializeObject<Config>(rawConfig);

            return CreateDiscordConfig(botConfig);
        }

        public DiscordConfiguration SetConfig(Config customConfig) => CreateDiscordConfig(customConfig);

        public DiscordConfiguration CreateDiscordConfig(Config config)
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
            var rawVibes = await DataRouter.ReadFromConfigs(SumeruVibes);

            var sumeruVibes = JsonConvert.DeserializeObject<List<string>>(rawVibes);
            if (sumeruVibes == null) return currentVibe;

            currentVibe = sumeruVibes[Random.Shared.Next(sumeruVibes.Count)];
            return currentVibe;
        }

    }
}
