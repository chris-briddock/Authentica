namespace Domain.Requests;

/// <summary>
/// Represents a request to send a confirmation email token.
/// </summary>
public sealed record SendConfirmEmailTokenRequest
{
    /// <summary>
    /// The user's email address.
    /// </summary>
    public string Email { get; set; } = default!;
}