using DSharpPlus.Entities;

namespace Eremite.Actions
{
    internal class HelpAction
    {
        public const string HelpBanner = "https://raw.githubusercontent.com/MentallyStable4sure/Eremite/main/content/about.png";
        public const string CommandsLink = "https://mentally-stable.gitbook.io/eremite/";
        public const string Site = "https://mentallystable4sure.dev/";
        public const string Github = "https://github.com/MentallyStable4sure/Eremite";
        public const string Server = "https://discord.gg/mentallystable4sure";

        public static Version Version = new Version(1, 6, 0);

        public static string GetMessage()
        {
            return $"**Discord bot** for Mini-games and Genshin related stuff. You can pull characters and use them to do activities, making it easier to buy in-game Genshin stuff or pills*" +
                    $"\n\n> **pills** are cross-product currency inside **Mentally Stable ecosystem**, you can spend it in any **MentallyStable4sure game/product** with our **Launcher**" +
                    $"\n\n> Commands: {CommandsLink}\n> Launcher: {Site}\n\n> Server/Support: {Server}\n> Source code: {Github}" +
                    $"\n\n[Current Release: {Version.Major}.{Version.Minor}.{Version.Build}]";
        }

        public static DiscordEmbedBuilder GetEmbed()
        {
            return new DiscordEmbedBuilder()
            {
                Color = DiscordColor.SpringGreen,
                ImageUrl = HelpBanner,
                Description = GetMessage()
            };
        }

        public static DiscordInteractionResponseBuilder GetInteractionResponse()
        {
            return new DiscordInteractionResponseBuilder().AddEmbed(GetEmbed());
        }
    }
}
