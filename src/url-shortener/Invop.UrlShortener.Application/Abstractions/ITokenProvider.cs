namespace Invop.UrlShortener.Application.Abstractions;

public interface ITokenProvider
{
    event EventHandler? ReachingRangeLimit;
    void AssignRange(int start, int end);
    void AssignRange(TokenRange tokenRange);
    long GetToken();
}
