using System.Data;
using Dapper;
using Npgsql;

namespace Invop.UrlShortener.TokenRangeService;

internal sealed class TokenRangeManager(NpgsqlDataSource dataSource)
{
    private const int DefaultRangeSize = 1000;
    private readonly string _sqlQuery =
    $$"""
             WITH max_end AS (
                 SELECT COALESCE(MAX("End"), 0) AS "MaxEnd" FROM "TokenRanges"
             )
             INSERT INTO "TokenRanges" ("MachineIdentifier", "Start", "End")
             SELECT 
                 @MachineIdentifier,
                 CASE WHEN "MaxEnd" = 0 THEN {{DefaultRangeSize}} ELSE "MaxEnd" + 1 END,
                 CASE WHEN "MaxEnd" = 0 THEN {{DefaultRangeSize * 2}} ELSE "MaxEnd" + {{DefaultRangeSize}} END
             FROM max_end
             RETURNING "Start", "End";
       """;

    public async Task<TokenRangeResponse> AssignRangeAsync(string machineIdentifier)
    {
        await using var connection = await dataSource.OpenConnectionAsync();
        await using var transaction = await connection.BeginTransactionAsync(IsolationLevel.Serializable);

        try
        {
            var result = await connection.QuerySingleAsync<TokenRangeResponse>(
                _sqlQuery,
                new { MachineIdentifier = machineIdentifier },
                transaction
            );

            if (result is null)
            {
                throw new FailedToAssignRangeException("Failed to assign range.");
            }

            await transaction.CommitAsync();
            return result;
        }
        catch
        {
            await transaction.RollbackAsync();
            throw;
        }
    }
}
