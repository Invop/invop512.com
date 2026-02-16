using Invop.UrlShortener.Infrastructure.Database;

namespace Invop.UrlShortener.Server.Extensions;

public static class MigrationExtensions
{
    public static async ValueTask ApplyMigrations(this IApplicationBuilder app)
    {
        using IServiceScope scope = app.ApplicationServices.CreateScope();

        var dbInitializer = scope.ServiceProvider.GetRequiredService<DatabaseInitializer>();
        await dbInitializer.InitializeAsync();
    }
}
