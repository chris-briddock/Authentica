using Api.Requests;

namespace Application.Activities;

/// <summary>
/// Represents an activity that occurs when a group is deleted.
/// </summary>
public sealed class DeleteRoleActivity : ActivityBase<DeleteRoleRequest>
{
    /// <summary>
    /// Gets or sets the payload containing the request data for deleting a group.
    /// </summary>
    public override DeleteRoleRequest Payload { get; set; } = default!;
}