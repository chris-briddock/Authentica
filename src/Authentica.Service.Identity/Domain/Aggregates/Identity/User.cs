using Domain.Contracts;
using Domain.ValueObjects;
using Microsoft.AspNetCore.Identity;

namespace Domain.Aggregates.Identity;

/// <summary>
/// Represents a user in the identity system with additional auditing and soft deletion properties.
/// </summary>
public sealed class User : IdentityUser<string>,
                           ISoftDeletableEntity<string>, 
                           IAuditableEntity<string>
{
    /// <summary>
    /// Gets or sets the unique identifier for the user.
    /// </summary>
    public override string Id { get; set; } = Guid.NewGuid().ToString();

    /// <summary>
    /// Gets or sets the last login time and date.
    /// </summary>
    public DateTime? LastLoginDateTime { get; set; } = default!;

    /// <summary>
    /// Gets or sets the last login ip address.
    /// </summary>
    public string? LastLoginIPAddress { get; set; } = default!;

    /// <summary>
    /// Gets or sets a value indicating whether the user is deleted.
    /// </summary>
    public bool IsDeleted { get; set; }

    /// <summary>
    /// Gets or sets the date and time in UTC when the user was deleted.
    /// </summary>
    public DateTime? DeletedOnUtc { get; set; }

    /// <summary>
    /// Gets or sets the identifier of the user who deleted this user.
    /// </summary>
    public string? DeletedBy { get; set; }

    /// <summary>
    /// Gets or sets the date and time in UTC when the user was created.
    /// </summary>
    public DateTime CreatedOnUtc { get; set; }

    /// <summary>
    /// Gets or sets the identifier of the user who created this user.
    /// </summary>
    public string CreatedBy { get; set; } = default!;

    /// <summary>
    /// Gets or sets the date and time in UTC when the user was last modified.
    /// </summary>
    public DateTime? ModifiedOnUtc { get; set; }

    /// <summary>
    /// Gets or sets the identifier of the user who last modified this user.
    /// </summary>
    public string? ModifiedBy { get; set; }

    /// <summary>
    /// Gets or sets the address of the user.
    /// </summary>
    public Address Address { get; set; } = default!;

    /// <summary>
    /// Gets or sets if the user has enabled application based TOTP codes.
    /// </summary>
    public bool TwoFactorAuthenticatorEnabled { get; set; } = false;

    /// <summary>
    /// Gets or sets the collection of user roles associated with the user.
    /// </summary>
    public ICollection<UserRole> UserRoles { get; set; } = default!;

    /// <summary>
    /// Gets or sets the collection of user claims associated with the user.
    /// </summary>
    public ICollection<UserClaim> UserClaims { get; set; } = default!;

    /// <summary>
    /// Gets or sets the collection of user-client application links associated with the user.
    /// </summary>
    public ICollection<UserClientApplication> UserClientApplications { get; set; } = default!;
}
