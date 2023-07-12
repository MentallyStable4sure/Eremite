
using Eremite.Actions;
using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Entities;
using DSharpPlus.CommandsNext.Converters;

namespace Eremite
{
    internal class HelperAboutFormatter : DefaultHelpFormatter
    {
        public HelperAboutFormatter(CommandContext ctx) : base(ctx)
        {
        }

        public override CommandHelpMessage Build()
        {
            return new CommandHelpMessage(embed: HelpAction.GetEmbed().Build());
        }
    }
}
