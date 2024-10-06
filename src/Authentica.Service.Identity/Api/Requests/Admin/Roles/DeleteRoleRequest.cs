namespace Api.Requests;

/// <summary>
/// Represents a request to delete a group.
/// </summary>
public sealed record DeleteRoleRequest
{
    /// <summary>
    /// Gets or sets the name of the group to be deleted.
    /// </summary>
    public string Name { get; set; } = default!;
}