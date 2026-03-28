using MySqlConnector;

namespace backend.Data
{
    public class MySqlConnectionFactory
    {
        private readonly IConfiguration _configuration;

        public MySqlConnectionFactory(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public MySqlConnection CreateConnection()
        {
            var connectionString = ResolveConnectionString();
            return new MySqlConnection(connectionString);
        }

        private string ResolveConnectionString()
        {
            var configured = _configuration.GetConnectionString("DefaultConnection");
            if (!string.IsNullOrWhiteSpace(configured))
            {
                return configured;
            }

            var mysqlUrl = _configuration["MYSQL_URL"] ?? _configuration["DATABASE_URL"];
            if (!string.IsNullOrWhiteSpace(mysqlUrl))
            {
                return BuildFromUrl(mysqlUrl);
            }

            var host = _configuration["MYSQLHOST"];
            var port = _configuration["MYSQLPORT"];
            var database = _configuration["MYSQLDATABASE"];
            var user = _configuration["MYSQLUSER"];
            var password = _configuration["MYSQLPASSWORD"];

            if (!string.IsNullOrWhiteSpace(host) &&
                !string.IsNullOrWhiteSpace(database) &&
                !string.IsNullOrWhiteSpace(user))
            {
                var builder = new MySqlConnectionStringBuilder
                {
                    Server = host,
                    Port = uint.TryParse(port, out var parsedPort) ? parsedPort : 3306,
                    Database = database,
                    UserID = user,
                    Password = password ?? string.Empty,
                    SslMode = MySqlSslMode.Preferred
                };

                return builder.ConnectionString;
            }

            throw new InvalidOperationException(
                "MySQL connection is not configured. Set ConnectionStrings__DefaultConnection or Railway MySQL environment variables.");
        }

        private static string BuildFromUrl(string mysqlUrl)
        {
            var uri = new Uri(mysqlUrl);
            var userInfo = uri.UserInfo.Split(':', 2);

            var builder = new MySqlConnectionStringBuilder
            {
                Server = uri.Host,
                Port = uri.Port > 0 ? (uint)uri.Port : 3306,
                Database = uri.AbsolutePath.TrimStart('/'),
                UserID = Uri.UnescapeDataString(userInfo[0]),
                Password = userInfo.Length > 1 ? Uri.UnescapeDataString(userInfo[1]) : string.Empty,
                SslMode = MySqlSslMode.Preferred
            };

            return builder.ConnectionString;
        }
    }
}
