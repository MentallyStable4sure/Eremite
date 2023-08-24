using Eremite.Actions;
using DSharpPlus.SlashCommands;

namespace Eremite.SlashCommands
{
    public sealed class HelpCommand : ApplicationCommandModule
    {
        [SlashCommand("about", "List of current commands, docs and links")]
        public async Task ShowAbout(InteractionContext context)
        {
            await context.CreateResponseAsync(HelpAction.GetInteractionResponse());
        }

        [SlashCommand("info", "List of current commands, docs and links")]
        public async Task ShowInfo(InteractionContext context) => await ShowAbout(context);
    }
}
