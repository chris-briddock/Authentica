using Api.Requests;

namespace Domain.Events;

/// <summary>
/// Represents an event that occurs when a new application secret is created.
/// </summary>
public sealed class CreatedApplicationSecretEvent : EventBase<CreateApplicationSecretRequest>
{
    /// <summary>
    /// Gets or sets the payload containing the request data for creating an application secret.
    /// </summary>
    public override CreateApplicationSecretRequest Payload { get; set; } = default!;
}