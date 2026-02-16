using SharedKernel.GuardClauses;

namespace Invop.UrlShortener.Application.Extensions;

public static class Base62EncodingExtensions
{
    private const string AlphaNumeric = "0123456789" +
        "ABCDEFGHIJKLMNOPQRSTUVWXYZ" +
        "abcdefghijklmnopqrstuvwxyz";

    public static string EncodeToBase62(this long token)
    {
        Guard.Against.NegativeOrZero(token, nameof(token), "Number must be a positive integer.");
        Guard.Against.OutOfRange(token, nameof(token), 1, long.MaxValue, "Number must be between 1 and long.MaxValue.");
        // Max length for long in base62 is 11 characters
        Span<char> buffer = stackalloc char[11];
        var index = buffer.Length;

        while (token > 0)
        {
            buffer[--index] = AlphaNumeric[(int)(token % 62)];
            token /= 62;
        }

        return new string(buffer[index..]);
    }
}
