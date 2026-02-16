using Invop.UrlShortener.Application.Urls.GetLongUrl;
using Invop.UrlShortener.Server.Extensions;
using Invop.UrlShortener.Server.Infrastructure;
using Mediator;
using Microsoft.AspNetCore.Mvc;

namespace Invop.UrlShortener.Server.Endpoints.Urls;

internal sealed class Redirect : IEndpoint
{
    public const string Name = "RedirectShortUrl";
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapGet(ApiEndpoints.Redirect, async ([FromRoute] string uniqueCode, IMediator mediator, CancellationToken cancellationToken) =>
        {
            var query = new GetLongUrlQuery(uniqueCode);
            var result = await mediator.Send(query, cancellationToken);
            return result.Match(
                success => Results.Redirect(success.LongUrl),
                failure => CustomResults.Problem(failure)
            );
        }).WithName(Name);
    }
}
