using FluentValidation;
using Invop.UrlShortener.Application.Abstractions;
using Invop.UrlShortener.Application.Abstractions.Behaviors;
using Microsoft.Extensions.DependencyInjection;

namespace Invop.UrlShortener.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {

        services.AddMediator(
            (options) =>
            {
                options.Assemblies = [typeof(DependencyInjection)];
                options.ServiceLifetime = ServiceLifetime.Scoped;
                options.PipelineBehaviors = [
                    typeof(ValidationBehavior<,>),
                    typeof(LoggingBehavior<,>)
                ];
            }
        );
        services.AddScoped<IUniqueCodeGenerator, UniqueCodeGenerator>();
        services.AddValidatorsFromAssembly(typeof(DependencyInjection).Assembly, includeInternalTypes: true);

        return services;
    }
}
