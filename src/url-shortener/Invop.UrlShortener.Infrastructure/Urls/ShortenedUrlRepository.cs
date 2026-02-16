using Dapper;
using Invop.UrlShortener.Domain.Urls;
using Npgsql;

namespace Invop.UrlShortener.Infrastructure.Urls;

internal sealed class ShortenedUrlRepository(NpgsqlDataSource npgsqlDataSource)
    : IShortenedUrlRepository
{
    public async Task<ShortenedUrl> AddAsync(ShortenedUrl shortenedUrl, CancellationToken cancellationToken = default)
    {
        const string sql = """
            insert into shortened_urls (unique_code, long_url, created_by, created_on)
            values (@UniqueCode, @LongUrl, @CreatedBy, @CreatedOn)
            """;

        using var connection = await npgsqlDataSource.OpenConnectionAsync(cancellationToken);
        await connection.ExecuteAsync(
            new CommandDefinition(
                sql,
                new
                {
                    UniqueCode = shortenedUrl.UniqueCode,
                    LongUrl = shortenedUrl.LongUrl,
                    CreatedBy = shortenedUrl.CreatedBy,
                    CreatedOn = shortenedUrl.CreatedOn
                },
                cancellationToken: cancellationToken
            )
        );
        return shortenedUrl;
    }

    public async Task<string?> GetByUniqueCodeAsync(string uniqueCode, CancellationToken cancellationToken = default)
    {
        const string sql = @"select long_url from shortened_urls where unique_code = @UniqueCode";
        using var connection = await npgsqlDataSource.OpenConnectionAsync(cancellationToken);
        return await connection.QuerySingleOrDefaultAsync<string>(
            new CommandDefinition(
                sql,
                new { UniqueCode = uniqueCode },
                cancellationToken: cancellationToken
            )
        );
    }

    public async Task<IReadOnlyList<ShortenedUrl>> GetByUserIdAsync(Guid userId, CancellationToken cancellationToken = default)
    {
        const string sql = """
            select unique_code as UniqueCode, long_url as LongUrl, created_by as CreatedBy, created_on as CreatedOn
            from shortened_urls
            where created_by = @UserId
            order by created_on desc
            """;

        using var connection = await npgsqlDataSource.OpenConnectionAsync(cancellationToken);
        var result = await connection.QueryAsync<ShortenedUrl>(
            new CommandDefinition(
                sql,
                new { UserId = userId },
                cancellationToken: cancellationToken
            )
        );
        return result.ToList();
    }
}