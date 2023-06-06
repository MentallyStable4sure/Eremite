using Eremite.Data.DiscordData;

namespace Eremite
{
    public static class Extensions
    {

        public static bool IsValid(this UserData user) => user.UserId != null && user.UserId != string.Empty;
    }
}
