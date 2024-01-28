
using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;
using Eremite.Services;

namespace Eremite.Commands
{
    public sealed class SlashTransferNotify : BaseCommandModule
    {
        public DataHandler DataHandler { get; set; }

        [Command("akasha")]
        public async Task Move1(CommandContext context) => await context.RespondAsync("Moved to slash commands /akasha or /profile");

        [Command("profile")]
        public async Task Move2(CommandContext context) => await context.RespondAsync("Moved to slash commands /akasha or /profile");

        [Command("adventure")]
        public async Task Move3(CommandContext context) => await context.RespondAsync("Moved to slash commands /adventure");

        [Command("daily")]
        public async Task Move4(CommandContext context) => await context.RespondAsync("Moved to slash commands /daily");

        [Command("shop")]
        public async Task Move5(CommandContext context) => await context.RespondAsync("Moved to slash commands /shop");
    }
}
