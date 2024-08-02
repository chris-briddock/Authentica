using Domain.Contracts;
using Microsoft.AspNetCore.Identity;

namespace Domain.Aggregates.Identity;

/// <summary>
/// Represents a claim associated with a user in the identity system.
/// </summary>
public sealed class UserClaim : IdentityUserClaim<string>,
                                ISoftDeletableEntity<string>,
                                IAuditableEntity<string>
                                
{
    /// <summary>
    /// Gets or sets the unique identifier for the user claim.
    /// </summary>
    public new string Id { get; set; } = Guid.NewGuid().ToString();

    /// <summary>
    /// Gets or sets the foreign key for a user.
    /// </summary>
    public override string UserId { get; set; } = default!;

    /// <summary>
    /// Gets or sets the user navigation property.
    /// </summary>
    public User User { get; set; } = default!;

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
    /// A random value that should change whenever a application is persisted.
    /// </summary>
    public string? ConcurrencyStamp { get; set; }
}