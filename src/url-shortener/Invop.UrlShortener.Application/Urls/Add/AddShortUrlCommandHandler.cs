using Invop.UrlShortener.Application.Abstractions;
using Invop.UrlShortener.Domain.Urls;
using Mediator;
using SharedKernel;

namespace Invop.UrlShortener.Application.Urls.Add;

internal sealed class AddShortUrlCommandHandler
    (IUniqueCodeGenerator uniqueCodeGenerator, IDateTimeProvider dateTimeProvider, IShortenedUrlRepository repository)
    : ICommandHandler<AddShortUrlCommand, Result<AddShortUrlResponse>>
{
    public async ValueTask<Result<AddShortUrlResponse>> Handle(AddShortUrlCommand command,
        CancellationToken cancellationToken)
    {
        var shortened = ShortenedUrl.Create(
            uniqueCodeGenerator.GenerateUniqueCode(),
            command.LongUri,
            command.UserId,
            dateTimeProvider.UtcNow
        );
        await repository.AddAsync(shortened, cancellationToken);
        return Result.Success(new AddShortUrlResponse(shortened.UniqueCode, shortened.LongUrl));
    }
}
