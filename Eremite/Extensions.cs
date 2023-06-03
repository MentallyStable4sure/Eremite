using DSharpPlus;
using Eremite.Data;

namespace Eremite
{
    public static class Extensions
    {
        public static DiscordConfiguration CreateDiscordConfig(this Config config)
        {
            return new DiscordConfiguration()
            {
                Token = config.Token,
                TokenType = config.TokenType
            };
        }
    }
}
