namespace Invop.UrlShortener.TokenRangeService;

[Serializable]
internal class FailedToAssignRangeException : Exception
{
    public FailedToAssignRangeException()
    {
    }

    public FailedToAssignRangeException(string? message) : base(message)
    {
    }

    public FailedToAssignRangeException(string? message, Exception? innerException) : base(message, innerException)
    {
    }
}