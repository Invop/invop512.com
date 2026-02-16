using SharedKernel.GuardClauses;

namespace Invop.UrlShortener.Domain.Urls;

public class ShortenedUrl
{
    public string UniqueCode { get; private set; }
    public Uri LongUrl { get; private set; }
    public Guid CreatedBy { get; private set; }
    public DateTime CreatedOn { get; private set; }

    private ShortenedUrl()
    {
    }
    private ShortenedUrl(string uniqueCode, Uri longUrl, Guid createdBy, DateTime createdOn)
    {
        UniqueCode = uniqueCode;
        LongUrl = longUrl;
        CreatedBy = createdBy;
        CreatedOn = createdOn;
    }

    public static ShortenedUrl Create(string uniqueCode, Uri longUrl, Guid createdBy, DateTime createdOn)
    {
        Guard.Against.Null(longUrl);
        Guard.Against.NullOrWhiteSpace(uniqueCode);
        Guard.Against.NullOrEmpty(createdBy);
        Guard.Against.NullOrOutOfSQLDateRange(createdOn);

        return new ShortenedUrl(uniqueCode, longUrl, createdBy, createdOn);
    }
}