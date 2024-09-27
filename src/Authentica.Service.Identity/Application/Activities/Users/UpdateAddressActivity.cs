using Api.Requests;

namespace Application.Activities;

/// <summary>
/// Represents an activity that occurs when a user's address is updated.
/// </summary>
public class UpdateAddressActivity : ActivityBase<UpdateAddressRequest>
{
    /// <summary>
    /// Gets or sets the payload containing the request data for updating a user's address.
    /// </summary>
    public override UpdateAddressRequest Payload { get; set; } = default!;
}
