using Api.Requests;

namespace Application.Activities;

/// <summary>
/// Represents an activity that occurs when a login request is made.
/// </summary>
public class LoginActivity : ActivityBase<LoginRequest>
{
    /// <summary>
    /// Gets or sets the payload containing the request data for the login activity.
    /// </summary>  
    public override LoginRequest Payload { get; set; } = default!;
}
