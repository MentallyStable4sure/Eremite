
namespace Eremite.Data.DiscordData
{
    [Serializable]
    public class DiscordWallet
    {
        public int Mora = 0;
        public int Primogems = 0;
        public int Pills = 0;


        public DiscordWallet() { }

        public DiscordWallet(int primogems, int mora, int pills = 0)
        {
            Primogems = primogems;
            Mora = mora;
            Pills = pills;
        }

        public override string ToString() => $"[{Primogems}] Primogems, [{Mora}] Mora, [{Pills}] 💊";
    }
}
