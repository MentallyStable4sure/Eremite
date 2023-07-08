using Eremite.Actions;
using Eremite.Services;
using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;

namespace Eremite.Commands
{
    public sealed class ConnectCommand : BaseCommandModule
    {
        public DataHandler DataHandler { get; set; }

        [Command("genshin"), Description("Connects your genshin UID")]
        public async Task ConnectGenshin(CommandContext context, string uid)
        {
            var action = new ConnectAction(DataHandler);

            var user = await DataHandler.GetData(context.User);
            if (!ConnectAction.CheckGenshinUID(uid))
            {
                await context.RespondAsync($"> UID Error. Example `!genshin 700000001`");
                return;
            }

            await action.ConnectGenshinUIDForUser(user, uid);

            await context.RespondAsync($"> {user.Username} UID: {user.Stats.UserUID}");
        }
    }
}
