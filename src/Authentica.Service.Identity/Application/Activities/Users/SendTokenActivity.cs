using Api.Requests;

namespace Application.Activities;

/// <summary>
/// Represents an activity that occurs when a user requests a token.
/// </summary>
public sealed class SendTokenActivity : ActivityBase<SendTokenRequest>
{
    /// <summary>
    /// Gets or sets the payload of the activity, which contains the details of the token request.
    /// </summary>
    public override SendTokenRequest Payload { get; set; } = default!;
}