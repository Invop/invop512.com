using Mediator;
using SharedKernel;

namespace Invop.UrlShortener.Application.Urls.GetLongUrl;

public record GetLongUrlQuery(string UniqueCode) : IQuery<Result<RedirectLinkResponse>>;
