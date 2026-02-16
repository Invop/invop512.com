using Invop.UrlShortener.Application.Extensions;

namespace Invop.UrlShortener.Unit.Tests;

public class Base62EncodingTests
{
    [Fact]
    public void EncodeToBase62_NegativeNumber_ShouldThrowArgumentException()
    {
        // Arrange
        long negativeNumber = -100;

        // Act & Assert
        Should.Throw<ArgumentException>(() => negativeNumber.EncodeToBase62());
    }

    [Fact]
    public void EncodeToBase62_Zero_ShouldThrowArgumentException()
    {
        // Arrange
        long zero = 0;

        // Act & Assert
        Should.Throw<ArgumentException>(() => zero.EncodeToBase62());
    }

    [Fact]
    public void EncodeToBase62_SmallNumber_ShouldReturnSingleCharacter()
    {
        // Arrange
        long smallNumber = 10;

        // Act
        var result = smallNumber.EncodeToBase62();

        // Assert
        result.ShouldBe("A");
    }

    [Fact]
    public void EncodeToBase62_MediumNumber_ShouldReturnTwoCharacters()
    {
        // Arrange
        long mediumNumber = 1000;

        // Act
        var result = mediumNumber.EncodeToBase62();

        // Assert
        result.ShouldBe("G8");
    }

    [Fact]
    public void EncodeToBase62_LargeNumber_ShouldReturnMultipleCharacters()
    {
        // Arrange
        long largeNumber = 1_000_000;

        // Act
        var result = largeNumber.EncodeToBase62();

        // Assert
        result.ShouldBe("4C92");
    }

    [Fact]
    public void EncodeToBase62_BeforeLowerBoundary_ShouldThrowArgumentException()
    {
        // Arrange
        long beforeLowerBoundary = 0;

        // Act & Assert
        Should.Throw<ArgumentException>(() => beforeLowerBoundary.EncodeToBase62());
    }

    [Fact]
    public void EncodeToBase62_AtLowerBoundary_ShouldReturnOne()
    {
        // Arrange
        long lowerBoundary = 1;

        // Act
        var result = lowerBoundary.EncodeToBase62();

        // Assert
        result.ShouldBe("1");
    }

    [Fact]
    public void EncodeToBase62_AfterLowerBoundary_ShouldReturnTwo()
    {
        // Arrange
        long afterLowerBoundary = 2;

        // Act
        var result = afterLowerBoundary.EncodeToBase62();

        // Assert
        result.ShouldBe("2");
    }

    [Fact]
    public void EncodeToBase62_BeforeUpperBoundary_ShouldReturnEncodedValue()
    {
        // Arrange
        var beforeUpperBoundary = long.MaxValue - 1;

        // Act
        var result = beforeUpperBoundary.EncodeToBase62();

        // Assert
        result.ShouldNotBeNullOrEmpty();
        result.Length.ShouldBeLessThanOrEqualTo(11);
    }

    [Fact]
    public void EncodeToBase62_AtUpperBoundary_ShouldReturnEncodedValue()
    {
        // Arrange
        var upperBoundary = long.MaxValue;

        // Act
        var result = upperBoundary.EncodeToBase62();

        // Assert
        result.ShouldNotBeNullOrEmpty();
        result.Length.ShouldBeLessThanOrEqualTo(11);
    }

    [Theory]
    [InlineData(62, "10")]
    [InlineData(63, "11")]
    [InlineData(3844, "100")]
    public void EncodeToBase62_KnownValues_ShouldReturnExpectedResult(long input, string expected)
    {
        // Act
        var result = input.EncodeToBase62();

        // Assert
        result.ShouldBe(expected);
    }

    [Fact]
    public void EncodeToBase62_ValidNumber_ShouldContainOnlyBase62Characters()
    {
        // Arrange
        long number = 123456789;
        const string validChars = "0123456789ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz";

        // Act
        var result = number.EncodeToBase62();

        // Assert
        result.ShouldAllBe(c => validChars.Contains(c));
    }
}