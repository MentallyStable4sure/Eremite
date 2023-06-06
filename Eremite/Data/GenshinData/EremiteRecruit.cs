
namespace Eremite.Data.GenshinData
{
    [Serializable]
    public class EremiteRecruit
    {
        public string Username;
        public ulong ClientId;
        public int Uid;

        public EremiteRecruit(string username, ulong client, int uid)
        {
            this.Username = username;
            ClientId = client;
            this.Uid = uid;
        }
    }
}
