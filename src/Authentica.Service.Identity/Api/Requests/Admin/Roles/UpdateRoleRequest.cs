namespace Api.Requests;

/// <summary>
/// Represents a request to update information about a specific role.
/// </summary>
public sealed record UpdateRoleRequest
{
    /// <summary>
    /// Gets or sets the name of the role to be updated.
    /// </summary>
    public string CurrentName { get; set; } = default!;
    /// <summary>
    /// Gets or sets the new name of the role.
    /// </summary>
    public string NewName { get; set; } = default!;
}