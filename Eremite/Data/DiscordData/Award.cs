
namespace Eremite.Data.DiscordData
{
    public class Award
    {
        public DiscordWallet CurrenciesToAdd = new DiscordWallet();

        public List<Character> CharactersToAdd = new List<Character>();

        public Award(DiscordWallet Currencies, List<Character> characters = null)
        {
            CurrenciesToAdd = Currencies;

            CharactersToAdd ??= characters;
        }
    }
}
