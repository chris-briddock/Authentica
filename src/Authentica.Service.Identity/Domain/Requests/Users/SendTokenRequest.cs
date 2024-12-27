using Microsoft.AspNetCore.Mvc;

namespace Domain.Requests;

/// <summary>
/// Represents a confirmation token request.
/// </summary>
public sealed record SendTokenRequest
{
    /// <summary>
    /// The user's email address.
    /// </summary>
    [FromQuery(Name = "email_address")]
    public string Email { get; set; } = default!;

    /// <summary>
    /// The type of token to be generated.
    /// Allowed values for this are: Authentica.Common.EmailTokenConstants
    /// </summary>
    [FromQuery(Name = "token_type")]
    public string TokenType { get; set; } = default!;

}
