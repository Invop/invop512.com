using Invop.UrlShortener.Application.Extensions;

namespace Invop.UrlShortener.Application.Abstractions;

internal interface IUniqueCodeGenerator
{
    string GenerateUniqueCode();
}
internal class UniqueCodeGenerator : IUniqueCodeGenerator
{
    private readonly ITokenProvider _tokenProvider;
    public UniqueCodeGenerator(ITokenProvider tokenProvider)
    {
        _tokenProvider = tokenProvider;
    }
    public string GenerateUniqueCode()
    {
        var token = _tokenProvider.GetToken();
        return token.EncodeToBase62();
    }
}