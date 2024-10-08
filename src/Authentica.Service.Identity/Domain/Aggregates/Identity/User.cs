using Domain.Contracts;
using Domain.ValueObjects;
using Microsoft.AspNetCore.Identity;

namespace Domain.Aggregates.Identity;

/// <summary>
/// Represents a user in the identity system with additional auditing and soft deletion properties.
/// </summary>
public sealed class User : IdentityUser<string>, IEntityDeletionStatus<string>
{
    /// <summary>
    /// Gets or sets the unique identifier for the user.
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
    /// Gets or sets the address of the user.
    /// </summary>
    public Address Address { get; set; } = default!;

    /// <summary>
    /// Gets or sets if the user has enabled application based TOTPs.
    /// </summary>
    public bool MultiFactorAuthenticatorEnabled { get; set; } = false;

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
