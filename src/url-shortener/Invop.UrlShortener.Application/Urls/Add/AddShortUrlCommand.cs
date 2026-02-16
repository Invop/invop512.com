using Mediator;
using SharedKernel;

namespace Invop.UrlShortener.Application.Urls.Add;

public record AddShortUrlCommand(Guid UserId, Uri LongUri) : ICommand<Result<AddShortUrlResponse>>;
