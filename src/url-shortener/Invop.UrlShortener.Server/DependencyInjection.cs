using System.Reflection;
using Invop.UrlShortener.Server.Extensions;
using Invop.UrlShortener.Server.Infrastructure;

namespace Invop.UrlShortener.Server;

public static class DependencyInjection
{

    public static IServiceCollection AddPresentation(this IServiceCollection services)
    {
        services.AddEndpointsApiExplorer();
        services.AddExceptionHandler<GlobalExceptionHandler>();
        services.AddProblemDetails();
        services.AddOpenApi();
        services.AddEndpoints(Assembly.GetExecutingAssembly());
        return services;
    }
}
