using Api.Requests;

namespace Application.Activities;

/// <summary>
/// Represents an activity that occurs when mfa is disabled.
/// </summary>
public sealed class DisableMultiFactorActivity : ActivityBase<DisableMultiFactorRequest>
{
    /// <summary>
    /// Gets or sets the payload containing the request.
    /// </summary>
    public override DisableMultiFactorRequest Payload { get; set; } = default!;
}
