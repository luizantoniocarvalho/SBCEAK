namespace SBCEAK.Infraestrutura
{
    public enum Database
    {
        ORACLE,
        POSTGRE,
        PROGRESS
    }

    public struct Connection
    {
        public string host;
        public string sid;
        public string user;
        public string password;
        public string port;
    }

    public class ConnectionString
    {
        public static string Build(Connection connection)
        {
            string strConnection = $"Data Source = (DESCRIPTION = (ADDRESS = (PROTOCOL = TCP)(HOST = {connection.host})(PORT = {connection.port}))(CONNECT_DATA = (SID = {connection.sid}))); User Id={connection.user}; Password = {connection.password}";
            return strConnection;
        }
    }
}