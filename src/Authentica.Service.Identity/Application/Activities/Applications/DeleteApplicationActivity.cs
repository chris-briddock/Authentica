using Api.Requests;

namespace Application.Activities;

/// <summary>
/// Represents an activity that occurs when an application is deleted.
/// </summary>
public sealed class DeleteApplicationActivity : ActivityBase<DeleteApplicationByNameRequest>
{
    /// <summary>
    /// Gets or sets the payload containing the request data for deleting an application by name.
    /// </summary>
    public override DeleteApplicationByNameRequest Payload { get; set; } = default!;
}
