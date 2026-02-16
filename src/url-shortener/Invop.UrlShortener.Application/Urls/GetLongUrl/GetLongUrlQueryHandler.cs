using Invop.UrlShortener.Domain.Urls;
using Mediator;
using SharedKernel;

namespace Invop.UrlShortener.Application.Urls.GetLongUrl;

internal sealed class GetLongUrlQueryHandler
    (IShortenedUrlRepository repository)
    : IQueryHandler<GetLongUrlQuery, Result<RedirectLinkResponse>>
{
    public async ValueTask<Result<RedirectLinkResponse>> Handle(GetLongUrlQuery query, CancellationToken cancellationToken)
    {
        var longUrl = await repository.GetByUniqueCodeAsync(query.UniqueCode, cancellationToken);
        if (longUrl is null)
        {
            return Result.Failure<RedirectLinkResponse>(new Error("Url.NotFound", "Url not found", ErrorType.Problem));
        }

        var response = new RedirectLinkResponse(longUrl);
        return Result.Success(response);
    }
}
