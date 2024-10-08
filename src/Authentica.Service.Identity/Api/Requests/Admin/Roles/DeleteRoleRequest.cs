using Microsoft.AspNetCore.Mvc;

namespace Api.Requests;

/// <summary>
/// Represents a request to delete a group.
/// </summary>
public sealed record DeleteRoleRequest
{
    /// <summary>
    /// Gets or sets the name of the group to be deleted.
    /// </summary>
    [FromQuery(Name = "name")] 
    public string Name { get; set; } = default!;
}