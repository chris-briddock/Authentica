namespace Application.Activities;

/// <summary>
/// Represents an activity for creating a device code.
/// </summary>
public sealed class CreateDeviceCodeActivity : ActivityBase<string>
{
    /// <summary>
    /// Gets or sets the payload for the create device code activity.
    /// </summary>
    public override string Payload { get; set; } = default!;
}