namespace Domain.Requests;

/// <summary>
/// Represents a mfa sign in request.
/// </summary>
public sealed record MultiFactorLoginRequest
{
    /// <summary>
    /// The mfa token
    /// </summary>
    public string Token { get; init; } = default!;
}
