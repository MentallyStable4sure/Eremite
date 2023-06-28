using DSharpPlus;
using DSharpPlus.Entities;
using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;
using Eremite.Services;
using Eremite.Actions;
using Eremite.Data.DiscordData;

namespace Eremite.Commands
{
    public sealed class StatsCommand : BaseCommandModule
    {
        public DataHandler DataHandler { get; set; }

        private readonly string topByPullsKey = "stats.top.by_pulls";
        private readonly string topByPrimosKey = "stats.top.by_primos";
        private readonly string topByPillsKey = "stats.top.by_pills";

        [Command("stats"), Description("Shows the current user eremite stats such as teapot times visited, daily challenges done, characters pulled, etc.")]
        public async Task ShowStats(CommandContext context)
        {
            var user = await DataHandler.GetData(context.User);
            var buttons = await CreateButtons(user, context);

            var messageBuilder = new DiscordMessageBuilder()
                .AddComponents(buttons.Keys)
                .WithEmbed(StatsAction.GetEmbedWithStats(context.User.AvatarUrl, user));

            await context.RespondAsync(messageBuilder);
        }

        private async Task<Dictionary<DiscordButtonComponent, string>> CreateButtons(UserData user, CommandContext context)
        {
            var topPulls = Guid.NewGuid().ToString();
            var topPrimos = Guid.NewGuid().ToString();
            var topPills = Guid.NewGuid().ToString();

            var topPullsButton = new DiscordButtonComponent(ButtonStyle.Success, topPulls, user.GetText(topByPullsKey));
            var topPrimosButton = new DiscordButtonComponent(ButtonStyle.Secondary, topPrimos, user.GetText(topByPrimosKey));
            var topPillsButton = new DiscordButtonComponent(ButtonStyle.Secondary, topPills, user.GetText(topByPillsKey));


            context.Client.ComponentInteractionCreated += async (sender, args) =>
            {
                if (args.User.Id.ToString() != user.UserId) return;

                if (args.Id == topPulls) await args.Interaction.CreateResponseAsync(InteractionResponseType.UpdateMessage, StatsAction.SortUsersInBuilder(user.Stats.Language, await StatsAction.GetTopUsers(DataHandler, SortMethod.Pulls)));
                if (args.Id == topPrimos) await args.Interaction.CreateResponseAsync(InteractionResponseType.UpdateMessage, StatsAction.SortUsersInBuilder(user.Stats.Language, await StatsAction.GetTopUsers(DataHandler, SortMethod.Primogems)));
                if (args.Id == topPills) await args.Interaction.CreateResponseAsync(InteractionResponseType.UpdateMessage, StatsAction.SortUsersInBuilder(user.Stats.Language, await StatsAction.GetTopUsers(DataHandler, SortMethod.Pills)));
            };

            var buttons = new Dictionary<DiscordButtonComponent, string>();
            buttons.Add(topPullsButton, topPulls);
            buttons.Add(topPrimosButton, topPrimos);
            buttons.Add(topPillsButton, topPills);

            return buttons;
        }
    }
}
