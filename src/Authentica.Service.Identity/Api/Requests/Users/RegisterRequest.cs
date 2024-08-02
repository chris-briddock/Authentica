using Application.Attributes;
using Domain.ValueObjects;
using Microsoft.AspNetCore.Identity;

namespace Api.Requests;
/// <summary>
/// Represents a user registering for the application.
/// </summary>
public sealed record RegisterRequest
{
    /// <summary>
    /// The user's email address.
    /// </summary>
    [PersonalData]
    public required string Email { get; set; } = default!;
    /// <summary>
    /// The user's password.
    /// </summary>
    [SensitiveData]
    public required string Password { get; set; } = default!;
    /// <summary>
    /// The user's phone number.
    /// </summary>
    [PersonalData]
    public string? PhoneNumber { get; set; } = default!;
    /// <summary>
    /// The user's address details.
    /// </summary>
    [PersonalData]
    public Address Address { get; set; } = default!;
}