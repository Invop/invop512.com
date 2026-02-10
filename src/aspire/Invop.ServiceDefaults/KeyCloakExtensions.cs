#pragma warning disable IDE0130 // Namespace does not match folder structure
using Microsoft.Extensions.DependencyInjection;

namespace Microsoft.Extensions.Hosting;
#pragma warning restore IDE0130 // Namespace does not match folder structure

public static class KeyCloakExtensions
{
    public static TBuilder AddKeyCloakAuthentication<TBuilder>(this TBuilder builder,
        string serviceName,
        string realmName,
        string audience) where TBuilder : IHostApplicationBuilder
    {
        builder.Services.AddAuthentication()
                .AddKeycloakJwtBearer(
                    serviceName: serviceName,
                    realm: realmName,
                    options =>
                    {
                        options.Audience = audience;
                        //TODO: For development only - disable HTTPS metadata validation
                        // In production, use explicit Authority configuration instead
                        if (builder.Environment.IsDevelopment())
                        {
                            options.RequireHttpsMetadata = false;
                        }
                    });

        return builder;
    }
}
