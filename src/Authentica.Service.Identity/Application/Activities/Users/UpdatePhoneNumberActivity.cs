using Api.Requests;

namespace Application.Activities;

/// <summary>
/// Represents an activity that occurs when a user's phone number is updated.
/// </summary>
public class UpdatePhoneNumberActivity : ActivityBase<UpdatePhoneNumberRequest>
{
    /// <summary>
    /// Gets or sets the payload containing the request data for updating the user's phone number.
    /// </summary>
    public override UpdatePhoneNumberRequest Payload { get; set; } = default!;
}
