using backend.Data;
using backend.Models;
using Dapper;

namespace backend.Repositories
{
    public class MySqlNoteRepository : INoteRepository
    {
        private readonly MySqlConnectionFactory _connectionFactory;

        public MySqlNoteRepository(MySqlConnectionFactory connectionFactory)
        {
            _connectionFactory = connectionFactory;
        }

        public async Task<IEnumerable<Note>> GetAll(string userName)
        {
            const string sql = @"
SELECT
    id AS Id,
    user_name AS UserName,
    title AS Title,
    content AS Content,
    created_at AS CreatedAt,
    updated_at AS UpdatedAt
FROM notes
WHERE user_name = @userName
ORDER BY created_at DESC;";

            await using var connection = _connectionFactory.CreateConnection();
            return await connection.QueryAsync<Note>(sql, new { userName });
        }

        public async Task<Note?> GetById(int id, string userName)
        {
            const string sql = @"
SELECT
    id AS Id,
    user_name AS UserName,
    title AS Title,
    content AS Content,
    created_at AS CreatedAt,
    updated_at AS UpdatedAt
FROM notes
WHERE id = @id AND user_name = @userName
LIMIT 1;";

            await using var connection = _connectionFactory.CreateConnection();
            return await connection.QuerySingleOrDefaultAsync<Note>(sql, new { id, userName });
        }

        public async Task<Note> Create(CreateNoteDto dto, string userName)
        {
            var now = DateTime.UtcNow;

            const string insertSql = @"
INSERT INTO notes (user_name, title, content, created_at, updated_at)
VALUES (@userName, @title, @content, @createdAt, @updatedAt);";

            await using var connection = _connectionFactory.CreateConnection();
            await connection.ExecuteAsync(insertSql, new
            {
                userName,
                title = dto.Title,
                content = dto.Content,
                createdAt = now,
                updatedAt = now
            });

            var id = await connection.QuerySingleAsync<int>("SELECT LAST_INSERT_ID();");

            return new Note
            {
                Id = id,
                UserName = userName,
                Title = dto.Title,
                Content = dto.Content,
                CreatedAt = now,
                UpdatedAt = now
            };
        }

        public async Task<Note?> Update(int id, UpdateNoteDto dto, string userName)
        {
            var now = DateTime.UtcNow;

            const string updateSql = @"
UPDATE notes
SET title = @title,
    content = @content,
    updated_at = @updatedAt
WHERE id = @id AND user_name = @userName;";

            await using var connection = _connectionFactory.CreateConnection();
            var affectedRows = await connection.ExecuteAsync(updateSql, new
            {
                id,
                userName,
                title = dto.Title,
                content = dto.Content,
                updatedAt = now
            });

            if (affectedRows == 0)
            {
                return null;
            }

            return await GetById(id, userName);
        }

        public async Task<bool> Delete(int id, string userName)
        {
            const string sql = @"
DELETE FROM notes
WHERE id = @id AND user_name = @userName;";

            await using var connection = _connectionFactory.CreateConnection();
            var affectedRows = await connection.ExecuteAsync(sql, new { id, userName });
            return affectedRows > 0;
        }
    }
}
