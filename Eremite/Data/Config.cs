
using DSharpPlus;

namespace Eremite.Data
{
    [Serializable]
    public class Config
    {
        public string Token = "your token goes here";
        public TokenType TokenType = TokenType.Bot;
        public char Prefix = '!';
    }
}
