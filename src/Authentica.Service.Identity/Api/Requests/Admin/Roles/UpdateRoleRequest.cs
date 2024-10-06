namespace Api.Requests;

/// <summary>
/// Represents a request to update information about a specific group.
/// </summary>
public sealed record UpdateRoleRequest
{
    /// <summary>
    /// Gets or sets the name of the group to be updated.
    /// </summary>
    public string Name { get; set; } = default!;
}