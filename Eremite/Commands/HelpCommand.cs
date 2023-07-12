using DSharpPlus;
using DSharpPlus.Entities;
using DSharpPlus.CommandsNext;
using Eremite.Actions;
using DSharpPlus.CommandsNext.Attributes;

namespace Eremite.Commands
{
    public sealed class HelpCommand : BaseCommandModule
    {

        [Command("about"), Description("List of current commands, docs and links")]
        public async Task ShowAbout(CommandContext context)
        {
            var messageBuilder = new DiscordMessageBuilder()
                .WithEmbed(HelpAction.GetEmbed());

            await context.RespondAsync(messageBuilder);
        }


        [Command("info"), Description("List of current commands, docs and links")]
        public async Task ShowInfo(CommandContext context) => await ShowAbout(context);
    }
}
