namespace Api.Requests;

/// <summary>
/// Represents a request to verify a device code in the Device Authorization Flow.
/// </summary>
public sealed class VerifyDeviceCodeRequest
{
    /// <summary>
    /// Gets or sets the device code to be verified.
    /// </summary>
    public string Code { get; set; } = default!;
}
