namespace Api.Requests;

/// <summary>
/// Represents a request to update a phone number.
/// </summary>
public sealed record UpdatePhoneNumberRequest
{
    /// <summary>
    /// Gets or sets the new phone number.
    /// </summary>
    public string PhoneNumber { get; set; } = default!;
    /// <summary>
    /// Gets or sets the token.
    /// </summary>
    public string Token { get; set; } = default!;
}
