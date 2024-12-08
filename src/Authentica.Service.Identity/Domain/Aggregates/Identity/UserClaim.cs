using Domain.Contracts;
using Domain.ValueObjects;
using Microsoft.AspNetCore.Identity;

namespace Domain.Aggregates.Identity;

/// <summary>
/// Represents a claim associated with a user in the identity system.
/// </summary>
public sealed class UserClaim :
    IdentityUserClaim<string>,
    IEntityDeletionStatus<string>,
    IEntityCreationStatus<string>,
    IEntityModificationStatus<string>
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
    /// Gets or sets the user navigation property.
    /// </summary>
    public User User { get; set; } = default!;
    /// <summary>
    /// A random value that should change whenever a application is persisted.
    /// </summary>
    public string? ConcurrencyStamp { get; set; }
}