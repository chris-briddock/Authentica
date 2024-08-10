using Domain.ValueObjects;

namespace Api.Responses;

/// <summary>
/// Represents the response for retrieving all users.
/// </summary>
public class GetUserResponse
{
    /// <summary>
    /// Gets or sets the user identifier.
    /// </summary>
    public string Id { get; set; } = default!;
    /// <summary>
    /// Gets or sets the user name.
    /// </summary>
    public string UserName { get; set; } = default!;

    /// <summary>
    /// Gets or sets the email address.
    /// </summary>
    public string Email { get; set; } = default!;

    /// <summary>
    /// Gets or sets a value indicating whether the email address is confirmed.
    /// </summary>
    public bool EmailConfirmed { get; set; } = default!;

    /// <summary>
    /// Gets or sets the phone number.
    /// </summary>
    public string PhoneNumber { get; set; } = default!;

    /// <summary>
    /// Gets or sets a value indicating whether the phone number is confirmed.
    /// </summary>
    public bool PhoneNumberConfirmed { get; set; } = default!;

    /// <summary>
    /// Gets or sets a value indicating whether two-factor authentication is enabled.
    /// </summary>
    public bool TwoFactorEnabled { get; set; } = default!;

    /// <summary>
    /// Gets or sets the date and time when the lockout ends.
    /// </summary>
    public DateTimeOffset? LockoutEnd { get; set; } = default!;

    /// <summary>
    /// Gets or sets a value indicating whether lockout is enabled.
    /// </summary>
    public bool LockoutEnabled { get; set; } = default!;

    /// <summary>
    /// Gets or sets the number of failed access attempts.
    /// </summary>
    public int AccessFailedCount { get; set; } = default!;

    /// <summary>
    /// Gets or sets the date and time of the last login.
    /// </summary>
    public DateTime? LastLoginDateTime { get; set; }

    /// <summary>
    /// Gets or sets the last login IP address.
    /// </summary>
    public string LastLoginIPAddress { get; set; } = default!;

    /// <summary>
    /// Gets or sets a value indicating whether the user is deleted.
    /// </summary>
    public bool IsDeleted { get; set; } = default!;

    /// <summary>
    /// Gets or sets the date and time when the user was deleted.
    /// </summary>
    public DateTime? DeletedOnUtc { get; set; }

    /// <summary>
    /// Gets or sets the identifier of the user who deleted this user.
    /// </summary>
    public string DeletedBy { get; set; } = default!;

    /// <summary>
    /// Gets or sets the date and time when the user was created.
    /// </summary>
    public DateTime CreatedOnUtc { get; set; }

    /// <summary>
    /// Gets or sets the identifier of the user who created this user.
    /// </summary>
    public string CreatedBy { get; set; } = default!;

    /// <summary>
    /// Gets or sets the date and time when the user was last modified.
    /// </summary>
    public DateTime? ModifiedOnUtc { get; set; } = default!;

    /// <summary>
    /// Gets or sets the identifier of the user who last modified this user.
    /// </summary>
    public string ModifiedBy { get; set; } = default!;

    /// <summary>
    /// Gets or sets the address of the user.
    /// </summary>
    public Address Address { get; set; } = default!;
}