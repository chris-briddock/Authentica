using Microsoft.AspNetCore.Identity;

namespace Domain.Aggregates.Identity;

/// <summary>
/// Represents the link between a user and a role in the identity system.
/// </summary>
public sealed class UserRole : IdentityUserRole<string>
{
    /// <summary>
    /// Gets or sets the role associated with this user-role relationship.
    /// </summary>
    public Role Role { get; set; } = default!;
    /// <summary>
    /// Gets or sets the user associated with this user-role relationship.
    /// </summary>
    public User User { get; set; } = default!;
}