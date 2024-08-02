using Microsoft.AspNetCore.Identity;

namespace Domain.Aggregates.Identity;

/// <summary>
/// Represents a claim associated with a role in the identity system.
/// </summary>
public sealed class RoleClaim : IdentityRoleClaim<string>
{
    /// <summary>
    /// Gets or sets the unique identifier for the role claim.
    /// </summary>
    public new string Id { get; set; } = Guid.NewGuid().ToString();

    /// <summary>
    /// A random value that should change whenever a role is persisted to the store.
    /// </summary>
    public string? ConcurrencyStamp { get; set; } = default!;

    /// <summary>
    /// Gets or sets the role associated with this claim.
    /// </summary>
    public Role Role { get; set; } = default!;
}
