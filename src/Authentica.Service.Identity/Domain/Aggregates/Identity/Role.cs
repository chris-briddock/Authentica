using Domain.Contracts;
using Domain.ValueObjects;
using Microsoft.AspNetCore.Identity;

namespace Domain.Aggregates.Identity;

/// <summary>
/// Represents a role in the identity system with additional auditing and soft deletion properties.
/// </summary>
public sealed class Role : 
    IdentityRole<string>,
    IEntityDeletionStatus<string>,
    IEntityCreationStatus<string>,
    IEntityModificationStatus<string>
{
    /// <summary>
    /// Gets or sets the unique identifier for the role.
    /// </summary>
    public override string Id { get; set; } = Guid.NewGuid().ToString();

    /// <summary>
    /// Gets or sets the creation status of the entity.
    /// </summary>
    /// <remarks>
    /// This property contains information about the creation of the entity.
    /// It includes whether the creation was successful and any relevant messages.
    /// </remarks>
    public EntityCreationStatus<string> EntityCreationStatus { get; set; } = default!;

    /// <summary>
    /// Gets or sets the deletion status of the entity.
    /// </summary>
    /// <remarks>
    /// This property tracks whether the entity has been soft-deleted, 
    /// along with metadata about the deletion event (like the timestamp and user responsible).
    /// </remarks>
    public EntityDeletionStatus<string> EntityDeletionStatus { get; set; } = default!;

    /// <summary>
    /// Gets or sets the modification status of the entity.
    /// </summary>
    /// <remarks>
    /// This property stores information about when the entity was created and last modified,
    /// and who performed the actions.
    /// </remarks>
    public EntityModificationStatus<string> EntityModificationStatus { get; set; } = default!;

    /// <summary>
    /// Gets or sets the collection of user roles associated with this role.
    /// </summary>
    public ICollection<UserRole> UserRoles { get; set; } = new List<UserRole>();

    /// <summary>
    /// Gets or sets the collection of claims associated with this role.
    /// </summary>
    public ICollection<RoleClaim> RoleClaims { get; set; } = new List<RoleClaim>();
}
