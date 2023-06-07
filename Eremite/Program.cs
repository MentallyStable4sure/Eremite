using DSharpPlus;
using Eremite.Services;
using DSharpPlus.Entities;
using DSharpPlus.CommandsNext;
using Microsoft.Extensions.DependencyInjection;
using Eremite.Commands;

namespace Eremite
{
    internal class Program
    {

        static async Task Main(string[] args)
        {
            var profileService = new BotProfileHandler();
            var dataHandler = new DataHandler();

            ServiceCollection services = new ServiceCollection();
            services.AddSingleton(profileService);
            services.AddSingleton(dataHandler);

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

    }
}