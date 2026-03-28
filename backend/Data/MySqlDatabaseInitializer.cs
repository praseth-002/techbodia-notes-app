using Dapper;

namespace backend.Data
{
    public class MySqlDatabaseInitializer
    {
        private readonly MySqlConnectionFactory _connectionFactory;

        public MySqlDatabaseInitializer(MySqlConnectionFactory connectionFactory)
        {
            _connectionFactory = connectionFactory;
        }

        public async Task EnsureDatabaseAsync(CancellationToken cancellationToken = default)
        {
            await using var connection = _connectionFactory.CreateConnection();
            await connection.OpenAsync(cancellationToken);

            const string sql = @"
CREATE TABLE IF NOT EXISTS notes (
    id INT NOT NULL AUTO_INCREMENT PRIMARY KEY,
    user_name VARCHAR(100) NOT NULL,
    title VARCHAR(200) NOT NULL,
    content TEXT NULL,
    created_at DATETIME(6) NOT NULL,
    updated_at DATETIME(6) NOT NULL,
    INDEX ix_notes_user_name_created_at (user_name, created_at)
);";

            await connection.ExecuteAsync(sql);
        }
    }
}
