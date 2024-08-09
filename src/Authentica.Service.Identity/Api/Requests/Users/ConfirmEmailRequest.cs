using Application.Attributes;
using Microsoft.AspNetCore.Mvc;

namespace Api.Requests;

/// <summary>
/// Represents an email confirmation.
/// </summary>
public sealed record ConfirmEmailRequest
{
    /// <summary>
    /// The users email address
    /// </summary>
    [FromQuery(Name = "email")]
    public required string Email { get; set; } = default!;
    /// <summary>
    /// The code to confirm the email address
    /// </summary>
    [FromQuery(Name = "token")]
    [SensitiveData]
    public required string Token { get; set; } = default!;

}
