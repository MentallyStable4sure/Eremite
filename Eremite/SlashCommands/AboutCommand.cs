using DSharpPlus.Entities;
using Eremite.Actions;
using DSharpPlus.SlashCommands;

namespace Eremite.Commands
{
    public sealed class AboutCommand : ApplicationCommandModule
    {

        [SlashCommand("about", "Shows info about the app")]
        public async Task ShowAbout(InteractionContext context)
        {
            var messageBuilder = new DiscordMessageBuilder()
                .WithEmbed(HelpAction.GetEmbed());


            await context.CreateResponseAsync(HelpAction.GetEmbed());
        }


        [SlashCommand("info", "Shows info about the app")]
        public async Task ShowInfo(InteractionContext context) => await ShowAbout(context);
    }
}