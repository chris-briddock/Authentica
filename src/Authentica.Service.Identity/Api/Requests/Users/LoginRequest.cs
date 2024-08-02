using Application.Attributes;
using Microsoft.AspNetCore.Identity;

namespace Api.Requests;

/// <summary>
/// Represents a request to log in to the application.
/// </summary>
public sealed record LoginRequest
{
    /// <summary>
    /// Gets or initializes the email address of the user.
    /// </summary>
    public string Email { get; init; } = default!;

    /// <summary>
    /// Gets or initializes the password of the user.
    /// </summary>
    [SensitiveData]
    public string Password { get; init; } = default!;

    /// <summary>
    /// Gets or initializes a value indicating whether the user should be remembered on this device.
    /// </summary>
    public bool RememberMe { get; init; }
}