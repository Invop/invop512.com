using FluentValidation;

namespace Invop.UrlShortener.Application.Urls.Add;

internal sealed class AddShortUrlCommandValidator : AbstractValidator<AddShortUrlCommand>
{
    public AddShortUrlCommandValidator()
    {
        RuleFor(x => x.LongUri)
            .Must(uri => uri.ToString().Length <= 2048);
    }
}
