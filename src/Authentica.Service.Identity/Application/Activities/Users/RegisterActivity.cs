using Api.Requests;

namespace Application.Activities;

/// <summary>
/// Represents an activity that occurs when a user registration request is made.
/// </summary>
public sealed class RegisterActivity : ActivityBase<RegisterRequest>
{
    /// <summary>
    /// Gets or sets the payload containing the request data for the user registration activity.
    /// </summary>
    public override RegisterRequest Payload { get; set; } = default!;
}
