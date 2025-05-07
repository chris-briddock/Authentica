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
    public new string Id { get; set; } = Gusid.New().ToString();

    /// <summary>
    /// A random value that should change whenever the entity is persisted.
    /// </summary>
    public string ConcurrencyStamp { get; set; } = Gusid.New().ToString();

    /// <summary>
    /// Gets or sets the role associated with this claim.
    /// </summary>
    public Role Role { get; set; } = default!;

}
