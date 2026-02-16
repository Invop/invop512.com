using System.Collections.Concurrent;
using System.Net.Http.Json;
using Bogus;
using Invop.UrlShortener.TokenRangeService;

namespace Invop.UrlShortener.Integrational.Tests.TokenRanges;

public class AssignTokenRangeTests : IClassFixture<TokenRangeServiceFixture>
{
    private readonly HttpClient _client;
    private readonly CancellationToken _cancellationToken = TestContext.Current.CancellationToken;

    public AssignTokenRangeTests(TokenRangeServiceFixture fixture)
    {
        _client = fixture.CreateClient();
    }

    [Fact]
    public async Task AssignTokenRange_WhenRequested_ShouldReturnRange()
    {
        // Arrange
        var faker = new Faker();
        var request = new AssignTokenRangeRequest(faker.Random.Word());

        // Act
        var response = await _client.PostAsJsonAsync("/assign", request, _cancellationToken);

        // Assert
        response.IsSuccessStatusCode.ShouldBeTrue();
        var tokenRange = await response.Content.ReadFromJsonAsync<TokenRangeResponse>(_cancellationToken);
        tokenRange!.Start.ShouldBeGreaterThan(0);
        tokenRange.End.ShouldBeGreaterThan(tokenRange.Start);
    }

    [Fact]
    public async Task AssignTokenRange_WhenRequestedTwice_ShouldNotRepeatRange()
    {
        // Arrange
        var faker = new Faker();
        var request = new AssignTokenRangeRequest(faker.Random.Word());

        // Act
        var response1 = await _client.PostAsJsonAsync("/assign", request, _cancellationToken);
        var response2 = await _client.PostAsJsonAsync("/assign", request, _cancellationToken);

        // Assert
        response1.IsSuccessStatusCode.ShouldBeTrue();
        response2.IsSuccessStatusCode.ShouldBeTrue();
        var tokenRange1 = await response1.Content.ReadFromJsonAsync<TokenRangeResponse>(_cancellationToken);
        var tokenRange2 = await response2.Content.ReadFromJsonAsync<TokenRangeResponse>(_cancellationToken);
        tokenRange2!.Start.ShouldBeGreaterThan(tokenRange1!.End);
    }

    [Fact]
    public async Task AssignTokenRange_WhenMultipleRequests_ShouldNotRepeatRanges()
    {
        // Arrange
        var faker = new Faker();
        var ranges = new ConcurrentBag<TokenRangeResponse>();

        // Act
        await Parallel.ForEachAsync(Enumerable.Range(1, 100), async (number, cancellationToken) =>
        {
            var request = new AssignTokenRangeRequest(faker.Random.Word() + number);
            var response = await _client.PostAsJsonAsync("/assign", request, cancellationToken);
            if (response.IsSuccessStatusCode)
            {
                var range = await response.Content.ReadFromJsonAsync<TokenRangeResponse>(cancellationToken: cancellationToken);
                ranges.Add(range!);
            }
        });

        // Assert
        ranges.Select(x => x.Start).ShouldBeUnique();
        ranges.Select(x => x.End).ShouldBeUnique();
    }
}
