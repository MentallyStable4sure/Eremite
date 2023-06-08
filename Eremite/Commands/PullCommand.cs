using Eremite.Actions;
using Eremite.Services;
using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;

namespace Eremite.Commands
{
    public sealed class PullCommand : BaseCommandModule
    {
        public DataHandler DataHandler { get; set; }
        public PullAction PullAction { get; set; }

        [Command("pull"), Description("Pull for a character X times")]
        public async Task PullCharacter(CommandContext context, int number)
        {
            var user = await DataHandler.GetData(context.User);

            if (user.Wallet.Primogems < DataHandler.Config.PullCost * number) await context.RespondAsync(PullAction.NotEnoughPrimosError);
            else
            {
                var charactersPulled = await PullAction.ForUserAsyncSave(user, number);

                await context.RespondAsync(StatsAction.GetEmbedWithCharacters(charactersPulled, user));
                await DataHandler.SendData(user, QueryHandler.GetUserUpdateCharactersAndWalletQuery(user));
            };
        }

        [Command("pull"), Description("Pull for a character onces")]
        public async Task PullCharacter(CommandContext context) => await PullCharacter(context, 1);
    }
}
