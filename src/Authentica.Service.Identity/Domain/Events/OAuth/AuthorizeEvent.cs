using Api.Requests;

namespace Domain.Events;

/// <summary>
/// Represents an event that occurs when an authorization request is made.
/// </summary>
public sealed class AuthorizeEvent : EventBase<AuthorizeRequest>
{
    /// <summary>
    /// Gets or sets the payload containing the request data for authorization.
    /// </summary>
    public override AuthorizeRequest Payload { get; set; } = default!;
}
