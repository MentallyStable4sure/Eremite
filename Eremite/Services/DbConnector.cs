
using Eremite.Data;
using MySql.Data.MySqlClient;

namespace Eremite.Services
{

    internal class DbConnector
    {
        protected string debugInfo = "[DbConnector]";

        public const string DbConfig = "dbconfig.json";

        public MySqlConnection Connection { get; protected set; }

        public event Action<MySqlConnection> OnConnectionOpened;
        public event Action OnConnectionClosed;


        public DbConnector(string host, int? port, string database, string username, string password)
        {
            string connection = $"Server={host};Port={port};Database={database};Uid={username};Pwd={password}";

            var mySqlConnection = new MySqlConnection(connection);
            Connection = mySqlConnection;
        }

        public DbConnector(DatabaseConfig config) : this(config.Host, config.Port, config.Database, config.Username, config.Password) { }

        public virtual async Task ConnectAsync()
        {
            if(Connection == null)
            {
                Console.WriteLine($"{debugInfo} Connection credentials not set, try calling DbConnector constructor");
                return;
            }

            await Connection.OpenAsync();
            OnConnectionOpened?.Invoke(Connection);
        }

        public virtual async Task CloseAndDisposeAsync()
        {
            if (Connection == null)
            {
                Console.WriteLine($"{debugInfo} Nothing to close, have u opened up a connection correctly?");
                return;
            }

            await Connection.CloseAsync();
            await Connection.DisposeAsync();

            OnConnectionClosed?.Invoke();
        }
    }
}
