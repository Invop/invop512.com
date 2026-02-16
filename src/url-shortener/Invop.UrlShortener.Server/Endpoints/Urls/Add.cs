using Invop.UrlShortener.Application.Urls.Add;
using Invop.UrlShortener.Server.Extensions;
using Invop.UrlShortener.Server.Infrastructure;
using Mediator;
using Microsoft.AspNetCore.Mvc;
namespace Invop.UrlShortener.Server.Endpoints.Urls;

internal sealed class Add : IEndpoint
{
    public const string Name = "CreateShortUrl";
    public record AddUrlRequest(Uri LongUrl);
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapPost(ApiEndpoints.Add, async ([FromBody] AddUrlRequest request, IMediator mediator, CancellationToken cancellationToken) =>
        {
            //TODO
            var command = new AddShortUrlCommand(Guid.NewGuid(), request.LongUrl);
            var result = await mediator.Send(command, cancellationToken);
            return result.Match(
                (x) => Results.CreatedAtRoute(Redirect.Name, new { uniqueCode = x.UniqueCode }, x),
                CustomResults.Problem);
        })
        .WithName(Name);
    }
}
