using Domain.Contracts;
using Microsoft.AspNetCore.Identity;

namespace Domain.Aggregates.Identity;

/// <summary>
/// Represents a role in the identity system with additional auditing and soft deletion properties.
/// </summary>
public sealed class Role : IdentityRole<string>, 
                           ISoftDeletableEntity<string>, 
                           IAuditableEntity<string>
{
    /// <summary>
    /// Gets or sets the unique identifier for the role.
    /// </summary>
    public override string Id { get; set; } = Guid.NewGuid().ToString();

    /// <summary>
    /// Gets or sets a value indicating whether the role is deleted.
    /// </summary>
    public bool IsDeleted { get; set; }

    /// <summary>
    /// Gets or sets the date and time in UTC when the role was deleted.
    /// </summary>
    public DateTime? DeletedOnUtc { get; set; }

    /// <summary>
    /// Gets or sets the identifier of the user who deleted this role.
    /// </summary>
    public string? DeletedBy { get; set; }

    /// <summary>
    /// Gets or sets the date and time in UTC when the role was created.
    /// </summary>
    public DateTime CreatedOnUtc { get; set; }

    /// <summary>
    /// Gets or sets the identifier of the user who created this role.
    /// </summary>
    public string CreatedBy { get; set; } = default!;

    /// <summary>
    /// Gets or sets the date and time in UTC when the role was last modified.
    /// </summary>
    public DateTime? ModifiedOnUtc { get; set; }

    /// <summary>
    /// Gets or sets the identifier of the user who last modified this role.
    /// </summary>
    public string? ModifiedBy { get; set; }

    /// <summary>
    /// Gets or sets the collection of user roles associated with this role.
    /// </summary>
    public ICollection<UserRole> UserRoles { get; set; } = default!;

    /// <summary>
    /// Gets or sets the collection of claims associated with this role.
    /// </summary>
    public ICollection<RoleClaim> RoleClaims { get; set; } = default!;
}
