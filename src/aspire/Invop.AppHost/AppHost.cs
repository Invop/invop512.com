#pragma warning disable IDE0059 // Unnecessary assignment of a value

var builder = DistributedApplication.CreateBuilder(args);

var db = builder.AddPostgres("postgres");

var keycloak = builder.AddKeycloak("keycloak", 8080)
    .WithRealmImport("./realms");

var cache = builder.AddRedis("cache");

var invop512 = builder.AddJavaScriptApp("invop512", "../../invop512.com")
    .WithNpm()
    .WithHttpEndpoint(env: "PORT")
    .WithExternalHttpEndpoints();

#region urlShortener

var urlShortenerTokenRangesDB = db.AddDatabase("urlShortener-token-rangesDB");

var urlShortenerTokenRangeService = builder
    .AddProject<Projects.Invop_UrlShortener_TokenRangeService>("urlShortener-token-range-service")
    .WithReference(urlShortenerTokenRangesDB)
    .WaitFor(urlShortenerTokenRangesDB);

var urlShortenerDB = db.AddDatabase("urlShortenerDB");

var urlShortenerServer = builder.AddProject<Projects.Invop_UrlShortener_Server>("urlShortener-server")
    .WithReference(keycloak)
    .WaitFor(keycloak)
    .WithReference(cache)
    .WaitFor(cache)
    .WithReference(urlShortenerTokenRangeService)
    .WaitFor(urlShortenerTokenRangeService)
    .WithReference(urlShortenerDB)
    .WaitFor(urlShortenerDB)
    .WithHttpHealthCheck("/health")
    .WithExternalHttpEndpoints();

var urlShortenerWebFrontend = builder.AddViteApp("urlShortener-frontend", "../../url-shortener/Invop.UrlShortener.Frontend")
    .WithNpm()
    .WithReference(keycloak)
    .WaitFor(keycloak)
    .WithReference(urlShortenerServer)
    .WaitFor(urlShortenerServer);

urlShortenerServer.PublishWithContainerFiles(urlShortenerWebFrontend, "wwwroot");
#endregion

builder.Build().Run();

