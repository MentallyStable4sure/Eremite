using Eremite.Actions;
using Eremite.Services;
using DSharpPlus;
using DSharpPlus.Entities;
using DSharpPlus.CommandsNext;
using Microsoft.Extensions.DependencyInjection;
using Eremite.Data;
using Newtonsoft.Json;

namespace Eremite
{
    internal class Program
    {

        static async Task Main(string[] args)
        {
            var localizationHandler = new Localization();
            await localizationHandler.InitPacketAsync();

            var databaseConfig = await GetDatabaseConfig();
            var profileService = new BotProfileHandler();
            var charactersHandler = new CharactersHandler(databaseConfig);
            await charactersHandler.InitializeCharacterList();

            var dataHandler = new DataHandler(databaseConfig);
            var pullAction = new PullAction(dataHandler);

            ServiceCollection services = new ServiceCollection();
            services.AddSingleton(profileService);
            services.AddSingleton(dataHandler);
            services.AddSingleton(pullAction);

            var discord = new DiscordClient(await profileService.SetConfig());
            DiscordActivity activity = await profileService.SetStatus();

            var commands = new CommandsNextConfiguration()
            {
                Services = services.BuildServiceProvider(),
                StringPrefixes = profileService.GetBotConfig().Prefixes
            };

            var commandsNext = discord.UseCommandsNext(commands);
            commandsNext.RegisterCommands(typeof(Program).Assembly);

            await discord.ConnectAsync(activity, UserStatus.Idle);
            await Task.Delay(-1);
        }

        private static async Task<DatabaseConfig> GetDatabaseConfig()
        {
            var rawConfig = await DataGrabber.GrabFromConfigs(DbConnector.DbConfig);

            rawConfig.LogStatus(DbConnector.DbConfig);

            return JsonConvert.DeserializeObject<DatabaseConfig>(rawConfig);
        }

    }
}