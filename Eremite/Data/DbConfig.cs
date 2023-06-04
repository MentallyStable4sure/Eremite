
namespace Eremite.Data
{
    [Serializable]
    public class DbConfig
    {
        public string Host = "localhost";
        public int? Port = null;
        public string Database;
        public string Username = "root";
        public string Password = "root";
    }
}
