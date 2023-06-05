using DSharpPlus;
using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;
using DSharpPlus.Entities;
using Eremite.Data.DiscordData;
using Eremite.Services;

namespace Eremite.Commands
{
    public sealed class Topup : BaseCommandModule
    {
        public DataRouter DataRouter { get; set; }

        private string userClicked = string.Empty;

        [Command("topup"), Description("Sets primogems and mora for ur ID in the database")]
        public async Task TopupBalance(CommandContext context)
        {
            var addPrimos = Guid.NewGuid();
            var addMora = Guid.NewGuid();

            userClicked = context.User.Id.ToString();

            var addPrimosButton = new DiscordButtonComponent(ButtonStyle.Success, addPrimos.ToString(), "Add 100 Primogems");
            var addMoraButton = new DiscordButtonComponent(ButtonStyle.Danger, addMora.ToString(), "Add 100 Mora");

            var messageBuilder = new DiscordMessageBuilder()
                .WithContent($"Click to add stats to database for profile {context.User.Username} [{context.User.Id}]")
                .AddComponents(addPrimosButton, addMoraButton);

            context.Client.ComponentInteractionCreated += async (client, args) =>
            {
                if (args.User.Id.ToString() != userClicked)
                {
                    await args.Interaction.CreateResponseAsync(InteractionResponseType.UpdateMessage, 
                        new DiscordInteractionResponseBuilder().WithContent("You are not the one who used !topup command"));
                    return;
                }

                var remoteData = await DataRouter.GetData(userClicked);

                var user = new UserData();
                if (remoteData.UserId != string.Empty) user = remoteData;

                user.UserId = userClicked;

                user.Wallet.Primogems += 100;
                user.Wallet.Mora += 100;

                await DataRouter.SendData(user);
            };

            await context.RespondAsync(messageBuilder);
        }
    }
}
