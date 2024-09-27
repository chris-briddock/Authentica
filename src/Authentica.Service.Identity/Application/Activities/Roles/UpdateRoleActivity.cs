using Api.Requests;

namespace Application.Activities;

/// <summary>
/// Represents an activity for updating a group.
/// </summary>
public class UpdateRoleActivity : ActivityBase<UpdateRoleRequest>
{
    /// <summary>
    /// Gets or sets the payload for the update group request.
    /// </summary>
    public override UpdateRoleRequest Payload { get; set; } = default!;
}
