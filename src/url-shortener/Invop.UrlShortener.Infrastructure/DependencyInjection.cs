using Dapper;
using Invop.UrlShortener.Application.Abstractions;
using Invop.UrlShortener.Domain.Urls;
using Invop.UrlShortener.Infrastructure.Database;
using Invop.UrlShortener.Infrastructure.Time;
using Invop.UrlShortener.Infrastructure.Token;
using Invop.UrlShortener.Infrastructure.Urls;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SharedKernel;
namespace Invop.UrlShortener.Infrastructure;

public static class DependencyInjection
{

    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        var connectionString = configuration.GetConnectionString("urlShortenerDB") ??
            throw new InvalidOperationException("Connection string 'urlShortenerDB' not found.");
        services.AddNpgsqlDataSource(connectionString);
        SqlMapper.AddTypeHandler(new DapperUriTypeHandler());
        services.AddSingleton(sp => new DatabaseInitializer(connectionString));
        services.AddSingleton<IDateTimeProvider, DateTimeProvider>();
        services.AddSingleton<ITokenProvider, TokenProvider>();
        services.AddSingleton<IShortenedUrlRepository, ShortenedUrlRepository>();

        return services;
    }
}
