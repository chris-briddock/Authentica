using Api.Requests;

namespace Application.Activities;

/// <summary>
/// Represents an activity that occurs when a role is added to a user.
/// </summary>
public sealed class AddToRoleActivity : ActivityBase<AddToRoleRequest>
{
    /// <summary>
    /// Gets or sets the payload containing the request data for adding a role to a user.
    /// </summary>
    public override AddToRoleRequest Payload { get; set; } = default!;
}
