namespace Domain.Requests;

/// <summary>
/// Represents a request to send a token for updating the phone number.
/// </summary>
public sealed record SendUpdatePhoneNumberTokenRequest
{
    /// <summary>
    /// The user's email address.
    /// </summary>
    public string Email { get; set; } = default!;
}