﻿using Eremite.Data;
using Eremite.Actions;
using Eremite.Services;
using Eremite.Base.Interfaces;
using Newtonsoft.Json;
using DSharpPlus;
using DSharpPlus.Entities;
using DSharpPlus.CommandsNext;
using Microsoft.Extensions.DependencyInjection;
using DSharpPlus.SlashCommands;
using Eremite.SlashCommands;

namespace Eremite
{
    internal class Program
    {

        static async Task Main(string[] args)
        {
            var localizationHandler = new Services.Localization();
            await localizationHandler.InitPacketAsync();

            var databaseConfig = await GetDatabaseConfig();
            var profileService = new BotProfileHandler();
            var charactersHandler = new CharactersHandler(databaseConfig);
            await charactersHandler.InitializeCharacterList();

            var dataHandler = new DataHandler(databaseConfig);
            var pullAction = new PullAction(dataHandler);

            var discord = new DiscordClient(await profileService.SetConfig());
            DiscordActivity activity = await profileService.SetStatus();

            var commands = new CommandsNextConfiguration()
            {
                Services = BindServices(profileService, dataHandler, pullAction),
                StringPrefixes = profileService.GetBotConfig().Prefixes
            };

            var slashCommands = new SlashCommandsConfiguration()
            {
                Services = BindServices(profileService, dataHandler, pullAction),
            };

            var commandsNext = discord.UseCommandsNext(commands);
            var slash = discord.UseSlashCommands(slashCommands);

            commandsNext.SetHelpFormatter<HelperAboutFormatter>(); //overriding default help method

            commandsNext.RegisterCommands(typeof(Program).Assembly); //registering usual commands
            slash.RegisterCommands(typeof(Program).Assembly); //registering slash commands

            await discord.ConnectAsync(activity, UserStatus.Idle);
            await Task.Delay(-1);
        }

        private static ServiceProvider BindServices(
            BotProfileHandler profileService, DataHandler dataHandler,
            PullAction pullAction)
        {
            var services = new ServiceCollection();

            services.AddSingleton(profileService);
            services.AddSingleton(dataHandler);
            services.AddSingleton(pullAction);

            return services.BuildServiceProvider();
        }

        private static async Task<DatabaseConfig> GetDatabaseConfig()
        {
            var rawConfig = await DataGrabber.GrabFromConfigs(DbConnector.DbConfig);

            rawConfig.LogStatus(DbConnector.DbConfig);

            return JsonConvert.DeserializeObject<DatabaseConfig>(rawConfig);
        }

    }
}