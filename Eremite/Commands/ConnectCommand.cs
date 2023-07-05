using DSharpPlus;
using DSharpPlus.Entities;
using DSharpPlus.EventArgs;
using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;
using Eremite.Actions;
using Eremite.Services;
using Eremite.Data.DiscordData;
using Eremite.Layouts;
using Eremite.Builders;
using Eremite.Data;

namespace Eremite.Commands
{
    public sealed class ConnectCommand : BaseCommandModule
    {
        public DataHandler DataHandler { get; set; }

        [Command("genshin"), Description("Connects your genshin UID")]
        public async Task ShowAkasha(CommandContext context, string uid)
        {
            var user = await DataHandler.GetData(context.User);
            if (uid.Length != 9) return;

            user.Stats.UserUID = uid;

            var updateQuery = new UserUpdateQueryBuilder(user, QueryElement.Stats).Build();
            await DataHandler.SendData(user, updateQuery);
            await context.RespondAsync($"> UID: {user.Stats.UserUID}");
        }

        [Command("welkin"), Description("Connects your genshin UID")]
        public async Task BuyWelkin(CommandContext context, string uid)
        {
            var user = await DataHandler.GetData(context.User);
            if (uid.Length != 9) return;

            user.Stats.UserUID = uid;

            await SellerAction.BuyWelkin(uid);
            await context.RespondAsync($"> UID: {user.Stats.UserUID}");
        }

        [Command("test"), Description("Connects your genshin UID")]
        public async Task BuyWelkin(CommandContext context)
        {
            await SellerAction.BuyWelkin("708617087");
            await context.RespondAsync($"> UID: 708617087");
        }
    }
}
