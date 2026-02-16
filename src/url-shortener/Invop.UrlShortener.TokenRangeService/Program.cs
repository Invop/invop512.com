using Invop.UrlShortener.TokenRangeService;

var builder = WebApplication.CreateBuilder(args);
builder.AddServiceDefaults();

builder.AddNpgsqlDataSource(connectionName: "urlShortener-token-rangesDB");
builder.Services.AddSingleton<DbInitializer>();
builder.Services.AddSingleton<TokenRangeManager>();
builder.Services.AddOpenApi();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

var dbInitializer = app.Services.GetRequiredService<DbInitializer>();
await dbInitializer.InitializeAsync();

app.UseHttpsRedirection();

app.MapDefaultEndpoints();
app.MapGet("/", () => "TokenRanges Service");
app.MapPost("/assign",
    async (AssignTokenRangeRequest request, TokenRangeManager manager) =>
    {
        var range = await manager.AssignRangeAsync(request.Key);

        return range;
    });
app.Run();
