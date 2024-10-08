using Api.Requests;

namespace Application.Activities;

/// <summary>
/// Represents an activity that occurs when an application is updated by name.
/// </summary>
public sealed class UpdateApplicationByNameActivity : ActivityBase<UpdateApplicationByNameRequest>
{
    /// <summary>
    /// Gets or sets the payload containing the request data for updating an application by name.
    /// </summary>
    public override UpdateApplicationByNameRequest Payload { get; set; } = default!;
}