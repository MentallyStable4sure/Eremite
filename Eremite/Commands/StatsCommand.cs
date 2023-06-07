using DSharpPlus;
using DSharpPlus.Entities;
using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;
using Eremite.Services;
using Eremite.Actions;

namespace Eremite.Commands
{
    public sealed class StatsCommand : BaseCommandModule
    {
        public DataHandler DataHandler { get; set; }

        [Command("stats"), Description("Shows the current user eremite stats such as teapot times visited, daily challenges done, characters pulled, etc.")]
        public async Task ShowStats(CommandContext context)
        {
            var buttons = CreateButtons(context);
            var user = await DataHandler.GetData(context.User);

            var messageBuilder = new DiscordMessageBuilder()
                .AddComponents(buttons.Keys)
                .WithEmbed(StatsAction.GetEmbedWithStats(context.User.AvatarUrl, user));

            await context.RespondAsync(messageBuilder);
        }

        private Dictionary<DiscordButtonComponent, string> CreateButtons(CommandContext context)
        {
            var topPulls = Guid.NewGuid().ToString();
            var topPrimos = Guid.NewGuid().ToString();
            var topMora = Guid.NewGuid().ToString();

            var topPullsButton = new DiscordButtonComponent(ButtonStyle.Success, topPulls, "TOP by Gacha");
            var topPrimosButton = new DiscordButtonComponent(ButtonStyle.Secondary, topPrimos, "TOP by Primos");
            var topMoraButton = new DiscordButtonComponent(ButtonStyle.Secondary, topMora, "TOP by Mora");

            var buttons = new Dictionary<DiscordButtonComponent, string>();
            buttons.Add(topPullsButton, topPulls);
            buttons.Add(topPrimosButton, topPrimos);
            buttons.Add(topMoraButton, topMora);

            return buttons;
        }
    }
}
