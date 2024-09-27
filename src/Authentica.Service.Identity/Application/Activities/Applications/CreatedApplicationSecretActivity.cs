using Api.Requests;

namespace Application.Activities;

/// <summary>
/// Represents an event that occurs when a new application secret is created.
/// </summary>
public sealed class CreatedApplicationSecretActivity : ActivityBase<CreateApplicationSecretRequest>
{
    /// <summary>
    /// Gets or sets the payload containing the request data for creating an application secret.
    /// </summary>
    public override CreateApplicationSecretRequest Payload { get; set; } = default!;
}