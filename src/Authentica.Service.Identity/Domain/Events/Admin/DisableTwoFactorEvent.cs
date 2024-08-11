using Api.Requests;

namespace Domain.Events;

/// <summary>
/// Represents an event that is triggered to read all applications.
/// </summary>
public class DisableTwoFactorEvent : EventBase<DisableTwoFactorRequest>
{
    /// <summary>
    /// Gets or sets the payload containing the request.
    /// </summary>
    /// <value>
    /// <see cref="DisableTwoFactorEvent"/> object representing the request.
    /// </value>
    public override DisableTwoFactorRequest Payload { get; set; } = default!;
}
