using SharedKernel.GuardClauses;

namespace Invop.UrlShortener.Application.Abstractions;

public record TokenRange
{
    public TokenRange(long start, long end)
    {
        Guard.Against.Expression(s => s > end, start, nameof(start));
        Start = start;
        End = end;
    }

    public long Start { get; }
    public long End { get; }
}
