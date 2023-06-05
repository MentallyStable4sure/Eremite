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
        public DataHandler DataHandler { get; set; }

        [Command("topup"), Description("Sets primogems and mora for ur ID in the database")]
        public async Task TopupBalance(CommandContext context)
        {
            var addPrimos = Guid.NewGuid();
            var addMora = Guid.NewGuid();

            var userClicked = context.User.Id.ToString();
            bool isClicked = false;

            var addPrimosButton = new DiscordButtonComponent(ButtonStyle.Success, addPrimos.ToString(), "Add 100 Primogems");
            var addMoraButton = new DiscordButtonComponent(ButtonStyle.Danger, addMora.ToString(), "Add 100 Mora");

            var messageBuilder = new DiscordMessageBuilder()
                .WithContent($"Click to add stats to database for profile {context.User.Username} [{context.User.Id}]")
                .AddComponents(addPrimosButton, addMoraButton);

            context.Client.ComponentInteractionCreated += async (client, args) =>
            {
                if (isClicked) return;
                isClicked = true; //to prevent button from being spammed while async request isnt completed

                if (args.User.Id.ToString() != userClicked) return;

                var remoteData = await DataHandler.GetData(userClicked);

                var user = new UserData();
                if (remoteData.UserId != string.Empty) user = remoteData;

                user.UserId = userClicked;

                user.Wallet.Primogems += 100;
                user.Wallet.Mora += 100;
                Console.WriteLine($"Adding funds to {user.UserId}");

                await DataHandler.SendData(user);
                await args.Interaction.CreateResponseAsync(InteractionResponseType.UpdateMessage,
                        new DiscordInteractionResponseBuilder().WithContent("Funds successfully added!"));
            };

            await context.RespondAsync(messageBuilder);
        }
    }
}
