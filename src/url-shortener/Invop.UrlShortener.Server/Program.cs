using Invop.UrlShortener.Application;
using Invop.UrlShortener.Infrastructure;
using Invop.UrlShortener.Server;
using Invop.UrlShortener.Server.Extensions;
using Invop.UrlShortener.Server.Token;

var builder = WebApplication.CreateBuilder(args);

builder.AddServiceDefaults();

builder.AddRedisClientBuilder("cache")
    .WithOutputCache();

builder.Services
    .AddApplication()
    .AddInfrastructure(builder.Configuration)
    .AddPresentation();

builder.Services.AddHttpClient("TokenRangeService",
    client =>
    {
        client.BaseAddress =
            new Uri(builder.Configuration["URLSHORTENER_TOKEN_RANGE_SERVICE_HTTPS"]!);
    });
builder.Services.AddSingleton<ITokenRangeApiClient, TokenRangeApiClient>();
builder.Services.AddHostedService<TokenManager>();

var app = builder.Build();

app.UseExceptionHandler();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    await app.ApplyMigrations();
}

app.UseOutputCache();
app.MapDefaultEndpoints();
app.MapEndpoints();

app.UseFileServer();

await app.RunAsync();
