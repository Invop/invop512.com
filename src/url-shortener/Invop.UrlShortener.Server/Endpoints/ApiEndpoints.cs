namespace Invop.UrlShortener.Server.Endpoints;

internal static class ApiEndpoints
{
    private const string Base = "/api";
    public const string Add = $"{Base}/urls";
    public const string Redirect = $"{Base}/r/{{uniqueCode}}";
    public const string GetMy = $"{Base}/urls/list/";
}
