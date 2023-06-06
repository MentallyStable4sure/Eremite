using DSharpPlus;
using DSharpPlus.Entities;
using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;
using Eremite.Services;
using Eremite.Data.DiscordData;

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
            var addMoraButton = new DiscordButtonComponent(ButtonStyle.Secondary, addMora.ToString(), "Add 100 Mora");

            var messageBuilder = new DiscordMessageBuilder()
                .WithContent($"Click to add stats to database for profile {context.User.Username} [{context.User.Id}]")
                .AddComponents(addPrimosButton, addMoraButton);

            context.Client.ComponentInteractionCreated += async (client, args) =>
            {
                if (isClicked) return;

                if (args.User.Id.ToString() != userClicked) return;
                isClicked = true; //to prevent button from being spammed while request isnt completed

                var remoteData = await DataHandler.GetData(context.User);

                var user = remoteData.IsValid() ? remoteData : new UserData();
                user.UserId = userClicked;

                if(args.Id == addPrimos.ToString()) user.Wallet.Primogems += 100;
                if(args.Id == addMora.ToString()) user.Wallet.Mora += 100;
                Console.WriteLine($"Adding funds to {user.UserId}");

                await DataHandler.SendData(user, QueryHandler.GetUserUpdateWalletQuery(user));
                await args.Interaction.CreateResponseAsync(InteractionResponseType.UpdateMessage,
                        new DiscordInteractionResponseBuilder().WithContent("Funds successfully added!"));
            };

            await context.RespondAsync(messageBuilder);
        }
    }
}
