using Dapper;
using Npgsql;

namespace Invop.UrlShortener.TokenRangeService;

internal sealed class DbInitializer(NpgsqlDataSource dataSource)
{
    public async Task InitializeAsync()
    {
        await using var connection = await dataSource.OpenConnectionAsync();
        var createTableQuery = @"
            CREATE TABLE IF NOT EXISTS ""TokenRanges"" (
                ""Id"" SERIAL PRIMARY KEY,
                ""MachineIdentifier"" VARCHAR(255) NOT NULL,
                ""Start"" BIGINT NOT NULL,
                ""End"" BIGINT NOT NULL
            );
        ";
        await connection.ExecuteAsync(createTableQuery);
    }
}
