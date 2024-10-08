using Api.Requests;

namespace Application.Activities;

/// <summary>
/// Represents an activity that occurs when an authorization request is made.
/// </summary>
public sealed class AuthorizeActivity : ActivityBase<AuthorizeRequest>
{
    /// <summary>
    /// Gets or sets the payload containing the request data for authorization.
    /// </summary>
    public override AuthorizeRequest Payload { get; set; } = default!;
}
