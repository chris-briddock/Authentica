using Api.Requests;

namespace Application.Activities;

/// <summary>
/// Represents an activity that occurs when a new role is created.
/// </summary>
public sealed class CreateRoleActivity : ActivityBase<CreateRoleRequest>
{
    /// <summary>
    /// Gets or sets the payload containing the request data for creating a role.
    /// </summary>
    public override CreateRoleRequest Payload { get; set; } = default!;
}