namespace Application.Contracts;

/// <summary>
/// Defines a contract for a scope provider.
/// </summary>
public interface IScopeProvider
{
    /// <summary>
    /// Parses a space-delimited scope string into a list of individual scopes.
    /// </summary>
    /// <param name="scopeString">The space-delimited string of scopes to parse.</param>
    /// <returns>An IList&lt;string&gt; containing individual scopes. Returns an empty list if the input is null, empty, or whitespace.</returns>
    IList<string> ParseScopes(string scopeString);
}
