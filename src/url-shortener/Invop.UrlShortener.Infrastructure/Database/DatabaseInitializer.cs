using DbUp;

namespace Invop.UrlShortener.Infrastructure.Database;

public sealed class DatabaseInitializer(string connectionString)
{
    public async ValueTask InitializeAsync()
    {
        EnsureDatabase.For.PostgresqlDatabase(connectionString);
        var upgrader = DeployChanges.To.PostgresqlDatabase(connectionString)
            .WithScriptsEmbeddedInAssembly(typeof(DatabaseInitializer).Assembly)
            .LogToConsole()
            .Build();
        if (upgrader.IsUpgradeRequired())
        {
            upgrader.PerformUpgrade();
        }
    }
}
