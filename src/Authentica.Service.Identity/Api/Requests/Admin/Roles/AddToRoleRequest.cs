namespace Api.Requests;

/// <summary>
/// Represents a request to add a user to a role.
/// </summary>
public sealed record AddToRoleRequest
{
    /// <summary>
    /// Gets or sets the email address of the user to be added to the role.
    /// </summary>
    public string Email { get; set; } = default!;

    /// <summary>
    /// Gets or sets the name of the role to which the user will be added.
    /// </summary>
    public string Role { get; set; } = default!;
}