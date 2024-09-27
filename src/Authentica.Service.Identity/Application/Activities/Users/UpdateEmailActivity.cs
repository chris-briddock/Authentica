using Api.Requests;

namespace Application.Activities;

/// <summary>
/// Represents an activity that occurs when a user's email address is updated.
/// </summary>
public class UpdateEmailActivity : ActivityBase<UpdateEmailRequest>
{
    /// <summary>
    /// Gets or sets the payload containing the request data for updating the user's email address.
    /// </summary>
    public override UpdateEmailRequest Payload { get; set; } = default!;
}
