using DSharpPlus.CommandsNext;
using DSharpPlus.Entities;
using DSharpPlus.EventArgs;
using DSharpPlus;
using Eremite.Builders;
using Eremite.Data.DiscordData;
using Eremite.Data;
using Eremite.Services;

namespace Eremite.Actions
{
    public class ConnectAction
    {
        private DataHandler data;

        public ConnectAction(DataHandler data)
        {
            this.data = data;
        }

        public bool IsUserGenshinUIDConnected(UserData user) => CheckGenshinUID(user.Stats.UserUID);

        public static bool CheckGenshinUID(string uid)
        {
            if (uid == null || uid == string.Empty) return false;
            if (uid.Length != 9) return false;

            return true;
        }

        public async Task ConnectGenshinUIDForUser(UserData user, string genshinUID)
        {
            user.Stats.UserUID = genshinUID;

            var updateQuery = new UserUpdateQueryBuilder(user, QueryElement.Stats).Build();
            await data.SendData(user, updateQuery);
        }
    }
}
