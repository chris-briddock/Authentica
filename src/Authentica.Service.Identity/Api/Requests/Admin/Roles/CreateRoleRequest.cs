namespace Api.Requests;

/// <summary>
/// Represents a request to create a new group.
/// </summary>
public sealed record CreateRoleRequest
{
    /// <summary>
    /// Gets or sets the name of the group to be created.
    /// </summary>
    public string Name { get; set; } = default!;
}