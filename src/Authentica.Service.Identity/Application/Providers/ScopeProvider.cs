using Application.Contracts;

namespace Application.Providers;

/// <summary>
/// Provides functionality to parse JWT scope strings into lists of individual scopes.
/// </summary>
public sealed class ScopeProvider : IScopeProvider
{
    /// <summary>
    /// The character used to separate individual scopes in the scope string.
    /// </summary>
    private static readonly char[] separator = [' '];

    /// <inheritdoc/>
    public IList<string> ParseScopes(string scopeString)
    {
        if (string.IsNullOrWhiteSpace(scopeString))
        {
            return new List<string>();
        }

        return scopeString.Split(separator, StringSplitOptions.RemoveEmptyEntries)
                          .Select(s => s.Trim())
                          .Where(s => !string.IsNullOrEmpty(s))
                          .ToList();
    }
}