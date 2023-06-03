using DSharpPlus;
using Eremite.Data;
using Newtonsoft.Json;

namespace Eremite
{
    internal class Program
    {
        static async Task Main(string[] args)
        {
            var rawConfig = await DataHandler.ReadFromConfigs(DataHandler.StartupConfig);
            var config = JsonConvert.DeserializeObject<Config>(rawConfig);

            Console.WriteLine(rawConfig);
            var discord = new DiscordClient(config?.CreateDiscordConfig());

            await discord.ConnectAsync();
            await Task.Delay(-1);
        }
    }
}