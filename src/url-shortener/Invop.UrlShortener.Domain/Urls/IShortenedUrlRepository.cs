using SharedKernel;

namespace Invop.UrlShortener.Domain.Urls;

/// <summary>
/// Repository interface for managing ShortenedUrl aggregates.
/// </summary>
public interface IShortenedUrlRepository : IRepository<ShortenedUrl>
{
    /// <summary>
    /// Adds a new shortened URL to the repository.
    /// </summary>
    /// <param name="shortenedUrl">The shortened URL to add.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>The added shortened URL.</returns>
    Task<ShortenedUrl> AddAsync(ShortenedUrl shortenedUrl, CancellationToken cancellationToken = default);

    /// <summary>
    /// Finds the long URL by its short code.
    /// </summary>
    /// <param name="uniqueCode">The short URL code to search for.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>The long URL as a string if found, null otherwise.</returns>
    Task<string?> GetByUniqueCodeAsync(string uniqueCode, CancellationToken cancellationToken = default);

    /// <summary>
    /// Gets all shortened URLs created by a specific user.
    /// </summary>
    /// <param name="userId">The user identifier.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>Collection of shortened URLs created by the user.</returns>
    Task<IReadOnlyList<ShortenedUrl>> GetByUserIdAsync(Guid userId, CancellationToken cancellationToken = default);
}
