using System.Data;
using GuestbookApp.Models;
using Microsoft.Data.Sqlite;

namespace GuestbookApp.Data;

public class GuestbookRepository : IGuestbookRepository
{
    private readonly string _connectionString;
    private readonly ILogger<GuestbookRepository> _logger;

    public GuestbookRepository(IConfiguration configuration, IWebHostEnvironment environment, ILogger<GuestbookRepository> logger)
    {
        _logger = logger;

        var configuredConn = configuration.GetConnectionString("GuestbookDb");
        string dbPath;

        if (!string.IsNullOrWhiteSpace(configuredConn))
        {
            var builder = new SqliteConnectionStringBuilder(configuredConn);
            dbPath = builder.DataSource;
            if (!Path.IsPathRooted(dbPath))
            {
                dbPath = Path.Combine(environment.ContentRootPath, dbPath);
            }
        }
        else
        {
            dbPath = Path.Combine(environment.ContentRootPath, "App_Data", "guestbook.db");
        }

        var directory = Path.GetDirectoryName(dbPath);
        if (!string.IsNullOrEmpty(directory) && !Directory.Exists(directory))
        {
            Directory.CreateDirectory(directory);
        }

        _connectionString = $"Data Source={dbPath}";
        InitializeDatabase();
    }

    private void InitializeDatabase()
    {
        try
        {
            using var connection = new SqliteConnection(_connectionString);
            connection.Open();

            var createTableSql = @"
                CREATE TABLE IF NOT EXISTS GuestEntries (
                    Id INTEGER PRIMARY KEY AUTOINCREMENT,
                    Name TEXT NOT NULL,
                    Address TEXT NOT NULL,
                    Message TEXT NULL,
                    CreatedAt TEXT NOT NULL
                );";

            using (var command = connection.CreateCommand())
            {
                command.CommandText = createTableSql;
                command.ExecuteNonQuery();
            }

            // Ensure Message column exists if table was created in an older schema
            try
            {
                using var alterCommand = connection.CreateCommand();
                alterCommand.CommandText = "ALTER TABLE GuestEntries ADD COLUMN Message TEXT NULL;";
                alterCommand.ExecuteNonQuery();
            }
            catch (SqliteException)
            {
                // Column already exists, safe to ignore
            }

            _logger.LogInformation("Database SQLite berhasil diinisialisasi pada {ConnectionString}", _connectionString);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Gagal menginisialisasi database SQLite.");
            throw;
        }
    }

    public async Task<IReadOnlyList<GuestEntry>> GetAllEntriesAsync()
    {
        var entries = new List<GuestEntry>();

        await using var connection = new SqliteConnection(_connectionString);
        await connection.OpenAsync();

        var sql = "SELECT Id, Name, Address, Message, CreatedAt FROM GuestEntries ORDER BY Id DESC";
        await using var command = connection.CreateCommand();
        command.CommandText = sql;

        await using var reader = await command.ExecuteReaderAsync();
        while (await reader.ReadAsync())
        {
            var entry = new GuestEntry
            {
                Id = reader.GetInt32(0),
                Name = reader.GetString(1),
                Address = reader.GetString(2),
                Message = reader.IsDBNull(3) ? null : reader.GetString(3),
                CreatedAt = DateTimeOffset.TryParse(reader.GetString(4), out var parsedDate)
                    ? parsedDate
                    : DateTimeOffset.Now
            };
            entries.Add(entry);
        }

        return entries;
    }

    public async Task AddEntryAsync(GuestEntry entry)
    {
        await using var connection = new SqliteConnection(_connectionString);
        await connection.OpenAsync();

        var sql = @"
            INSERT INTO GuestEntries (Name, Address, Message, CreatedAt)
            VALUES (@name, @address, @message, @createdAt);";

        await using var command = connection.CreateCommand();
        command.CommandText = sql;
        command.Parameters.AddWithValue("@name", entry.Name);
        command.Parameters.AddWithValue("@address", entry.Address);
        command.Parameters.AddWithValue("@message", (object?)entry.Message ?? DBNull.Value);
        command.Parameters.AddWithValue("@createdAt", entry.CreatedAt.ToString("O"));

        await command.ExecuteNonQueryAsync();
    }

    public async Task<int> GetTotalCountAsync()
    {
        await using var connection = new SqliteConnection(_connectionString);
        await connection.OpenAsync();

        var sql = "SELECT COUNT(*) FROM GuestEntries";
        await using var command = connection.CreateCommand();
        command.CommandText = sql;

        var result = await command.ExecuteScalarAsync();
        return Convert.ToInt32(result);
    }
}
