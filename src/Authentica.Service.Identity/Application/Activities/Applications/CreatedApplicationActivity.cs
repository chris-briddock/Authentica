using Api.Requests;

namespace Application.Activities;

/// <summary>
/// Represents an activity that occurs when a new application is created.
/// </summary>
public sealed class CreatedApplicationActivity : ActivityBase<CreateApplicationRequest>
{
    /// <summary>
    /// Gets or sets the payload containing the request data for creating an application.
    /// </summary>
    public override CreateApplicationRequest Payload { get; set; } = default!;
}
