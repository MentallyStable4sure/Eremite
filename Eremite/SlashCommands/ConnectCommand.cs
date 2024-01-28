using Eremite.Actions;
using Eremite.Services;
using DSharpPlus.SlashCommands;

namespace Eremite.SlashCommands
{
    public sealed class ConnectCommand : ApplicationCommandModule
    {
        public DataHandler DataHandler { get; set; }

        [SlashCommand("genshin", "Connects your genshin UID")]
        public async Task ConnectGenshin(InteractionContext context, [Option("uid", "Your genshin UID")] string uid)
        {
            var action = new ConnectAction(DataHandler);

            var user = await DataHandler.GetData(context.User);
            if (!ConnectAction.CheckGenshinUID(uid))
            {
                await context.CreateResponseAsync($"> ✖️ UID Error. Example `!genshin 700000001` ✖️");
                return;
            }

            await action.ConnectGenshinUIDForUser(user, uid);

            await context.CreateResponseAsync($"> ✨{user.Username} | UID: {user.Stats.UserUID} ✅");
        }

        [SlashCommand("connectgenshin", "Connects your genshin UID")]
        public async Task ConnectGenshinUID(InteractionContext context, [Option("uid", "Your genshin UID")]  string uid) => await ConnectGenshin(context, uid);
    }
}
