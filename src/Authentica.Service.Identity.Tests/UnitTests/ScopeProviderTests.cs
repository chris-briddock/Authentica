namespace Authentica.Service.Identity.Tests.UnitTests;

public class ScopeProviderTests
{
    private ScopeProvider _scopeProvider;

    [SetUp]
    public void Setup()
    {
        _scopeProvider = new ScopeProvider();
    }

    [Test]
    public void ParseScopes_ShouldReturnEmptyList_WhenScopeStringIsEmpty()
    {
        // Arrange
        var scopeString = "";

        // Act
        var result = _scopeProvider.ParseScopes(scopeString);

        // Assert
        Assert.That(result, Is.Empty);
    }

    [Test]
    public void ParseScopes_ShouldReturnEmptyList_WhenScopeStringIsWhitespace()
    {
        // Arrange
        var scopeString = "   ";

        // Act
        var result = _scopeProvider.ParseScopes(scopeString);

        // Assert
        Assert.That(result, Is.Empty);
    }

    [Test]
    public void ParseScopes_ShouldReturnSingleScope_WhenScopeStringContainsOneScope()
    {
        // Arrange
        var scopeString = "read";

        // Act
        var result = _scopeProvider.ParseScopes(scopeString);

        // Assert
        Assert.That(result, Is.Not.Empty);
        Assert.That(result, Has.Count.EqualTo(1));
        Assert.That(result[0], Is.EqualTo("read"));
    }

    [Test]
    public void ParseScopes_ShouldReturnMultipleScopes_WhenScopeStringContainsMultipleScopesSeparatedBySpaces()
    {
        // Arrange
        var scopeString = "read write delete";

        // Act
        var result = _scopeProvider.ParseScopes(scopeString);

        // Assert
        Assert.That(result, Has.Count.EqualTo(3));
        Assert.That(result, Is.EquivalentTo(new List<string> { "read", "write", "delete" }));
    }

    [Test]
    public void ParseScopes_ShouldTrimScopes_WhenScopeStringContainsWhitespaceAroundScopes()
    {
        // Arrange
        var scopeString = "  read  write  ";

        // Act
        var result = _scopeProvider.ParseScopes(scopeString);

        // Assert
        Assert.That(result, Has.Count.EqualTo(2));
        Assert.That(result, Is.EquivalentTo(new List<string> { "read", "write" }));
    }

    [Test]
    public void ParseScopes_ShouldRemoveEmptyEntries_WhenScopeStringContainsExtraSpaces()
    {
        // Arrange
        var scopeString = "read   write   delete";

        // Act
        var result = _scopeProvider.ParseScopes(scopeString);

        // Assert
        Assert.That(result, Has.Count.EqualTo(3));
        Assert.That(result, Is.EquivalentTo(new List<string> { "read", "write", "delete" }));
    }
}