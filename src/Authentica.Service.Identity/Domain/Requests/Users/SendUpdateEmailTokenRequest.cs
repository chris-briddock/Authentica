namespace Domain.Requests;

/// <summary>
/// Represents a request to send a token for updating the email.
/// </summary>
public sealed class SendUpdateEmailTokenRequest
{
    /// <summary>
    /// The user's email address.
    /// </summary>
    public string Email { get; set; } = default!;
}