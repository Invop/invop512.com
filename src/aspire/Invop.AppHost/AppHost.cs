#pragma warning disable IDE0059 // Unnecessary assignment of a value
using Aspire.Hosting;

var builder = DistributedApplication.CreateBuilder(args);

var keycloak = builder.AddKeycloak("keycloak", 8080)
    .WithRealmImport("./realms");

var cache = builder.AddRedis("cache");

var invop512 = builder.AddJavaScriptApp("invop512", "../../invop512.com")
    .WithNpm()
    .WithHttpEndpoint(env: "PORT")
    .WithExternalHttpEndpoints();

var urlShortenerServer = builder.AddProject<Projects.Invop_UrlShortener_Server>("urlShortenerServer")
    .WithReference(keycloak)
    .WaitFor(keycloak)
    .WithReference(cache)
    .WaitFor(cache)
    .WithHttpHealthCheck("/health")
    .WithExternalHttpEndpoints();

var urlShortenerWebFrontend = builder.AddViteApp("urlShortenerWebFrontend", "../../url-shortener/Invop.UrlShortener.Frontend")
    .WithNpm()
    .WithReference(keycloak)
    .WaitFor(keycloak)
    .WithReference(urlShortenerServer)
    .WaitFor(urlShortenerServer);

urlShortenerServer.PublishWithContainerFiles(urlShortenerWebFrontend, "wwwroot");

builder.Build().Run();

