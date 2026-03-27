using backend.Models;
using Dapper;
using Microsoft.Data.SqlClient;

namespace backend.Repositories
{
    public class NoteRepository : INoteRepository
    {
        private readonly string _connectionString;

        public NoteRepository(IConfiguration config)
        {
            _connectionString = config.GetConnectionString("DefaultConnection")
                ?? throw new InvalidOperationException("Missing ConnectionStrings:DefaultConnection");
            EnsureSchema();
        }

        private SqlConnection GetConnection() => new(_connectionString);

        private void EnsureSchema()
        {
            const string sql = @"
                IF NOT EXISTS (
                    SELECT 1
                    FROM INFORMATION_SCHEMA.TABLES
                    WHERE TABLE_NAME = 'Notes'
                )
                BEGIN
                    CREATE TABLE Notes (
                        Id INT IDENTITY(1,1) PRIMARY KEY,
                        UserName NVARCHAR(100) NOT NULL,
                        Title NVARCHAR(200) NOT NULL,
                        Content NVARCHAR(MAX) NULL,
                        CreatedAt DATETIME2 NOT NULL,
                        UpdatedAt DATETIME2 NOT NULL
                    );

                    CREATE INDEX IX_Notes_UserName_CreatedAt
                        ON Notes (UserName, CreatedAt DESC);
                END
            ";

            using var conn = GetConnection();
            conn.Execute(sql);
        }

        public async Task<IEnumerable<Note>> GetAll(string userName)
        {
            using var conn = GetConnection();
            return await conn.QueryAsync<Note>(
                "SELECT * FROM Notes WHERE UserName = @UserName ORDER BY CreatedAt DESC",
                new { UserName = userName });
        }

        public async Task<Note?> GetById(int id, string userName)
        {
            using var conn = GetConnection();
            return await conn.QueryFirstOrDefaultAsync<Note>(
                "SELECT * FROM Notes WHERE Id = @Id AND UserName = @UserName",
                new { Id = id, UserName = userName });
        }

        public async Task<Note> Create(CreateNoteDto dto, string userName)
        {
            using var conn = GetConnection();
            var now = DateTime.UtcNow;
            var sql = @"
                INSERT INTO Notes (UserName, Title, Content, CreatedAt, UpdatedAt)
                OUTPUT INSERTED.*
                VALUES (@UserName, @Title, @Content, @CreatedAt, @UpdatedAt)";

            return await conn.QuerySingleAsync<Note>(sql, new
            {
                UserName = userName,
                Title = dto.Title,
                Content = dto.Content,
                CreatedAt = now,
                UpdatedAt = now
            });
        }

        public async Task<Note?> Update(int id, UpdateNoteDto dto, string userName)
        {
            using var conn = GetConnection();
            var sql = @"
                UPDATE Notes
                SET Title = @Title, Content = @Content, UpdatedAt = @UpdatedAt
                OUTPUT INSERTED.*
                WHERE Id = @Id AND UserName = @UserName";

            return await conn.QueryFirstOrDefaultAsync<Note>(sql, new
            {
                Id = id,
                UserName = userName,
                Title = dto.Title,
                Content = dto.Content,
                UpdatedAt = DateTime.UtcNow
            });
        }

        public async Task<bool> Delete(int id, string userName)
        {
            using var conn = GetConnection();
            var rows = await conn.ExecuteAsync(
                "DELETE FROM Notes WHERE Id = @Id AND UserName = @UserName",
                new { Id = id, UserName = userName });
            return rows > 0;
        }
    }
}