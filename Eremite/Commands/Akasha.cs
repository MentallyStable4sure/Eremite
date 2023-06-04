using DSharpPlus;
using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;
using DSharpPlus.Entities;

namespace Eremite.Commands
{
    public sealed class Akasha : BaseCommandModule
    {
        [Command("akasha"), Description("Shows the current user profile with the current equipped character, mora and primos")]
        public async Task ShowProfile(CommandContext context)
        {
            var shopGuid = Guid.NewGuid();
            var pullGuid = Guid.NewGuid();

            var shardId = context.Client.ShardId;
            var userId = context.User.Id;

            var pullButton = new DiscordButtonComponent(ButtonStyle.Success, pullGuid.ToString(), "Pull");
            var shopButton = new DiscordButtonComponent(ButtonStyle.Secondary, shopGuid.ToString(), "Mora Shop");

            var messageBuilder = new DiscordMessageBuilder()
                .WithContent($"Here is your profile ${context.User.Username} [{context.User.Id}]");
            messageBuilder.AddComponents(pullButton, shopButton);

            await context.RespondAsync(messageBuilder);
        }
    }
}
