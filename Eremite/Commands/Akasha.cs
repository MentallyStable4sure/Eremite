using DSharpPlus;
using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;
using DSharpPlus.Entities;
using Eremite.Services;

namespace Eremite.Commands
{
    public sealed class Akasha : BaseCommandModule
    {
        public DataHandler DataHandler { get; set; }

        [Command("akasha"), Description("Shows the current user profile with the current equipped character, mora and primos")]
        public async Task ShowProfile(CommandContext context)
        {
            var shopGuid = Guid.NewGuid();
            var pullGuid = Guid.NewGuid();

            var userId = context.User.Id;

            var user = await DataHandler.GetData(userId.ToString());

            var pullButton = new DiscordButtonComponent(ButtonStyle.Success, pullGuid.ToString(), "Pull");
            var shopButton = new DiscordButtonComponent(ButtonStyle.Secondary, shopGuid.ToString(), "Mora Shop");

            var messageBuilder = new DiscordMessageBuilder()
                .WithContent($"Here is your profile {context.User.Username} [{user.UserId}]" +
                $"\nSaved primogems: {user.Wallet.Primogems} | Saved mora: {user.Wallet.Mora}")
                .AddComponents(pullButton, shopButton);

            await context.RespondAsync(messageBuilder);
        }
    }
}
