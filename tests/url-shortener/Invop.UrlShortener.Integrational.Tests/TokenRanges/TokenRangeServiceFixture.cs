using Invop.UrlShortener.TokenRangeService;
using Microsoft.AspNetCore.Mvc.Testing;
using Testcontainers.PostgreSql;

namespace Invop.UrlShortener.Integrational.Tests.TokenRanges;

public class TokenRangeServiceFixture : WebApplicationFactory<ITokenRangeAssemblyMarker>, IAsyncLifetime
{
    private readonly PostgreSqlContainer _postgresContainer;
    private string ConnectionString => _postgresContainer.GetConnectionString();

    public TokenRangeServiceFixture()
    {
        _postgresContainer = new PostgreSqlBuilder("postgres:latest").Build();
    }

    public async ValueTask InitializeAsync()
    {
        await _postgresContainer.StartAsync();

        Environment.SetEnvironmentVariable("ConnectionStrings__urlShortener-token-rangesDB", ConnectionString);

    }

    public new async ValueTask DisposeAsync()
    {
        await _postgresContainer.StopAsync();
        await base.DisposeAsync();
    }
}