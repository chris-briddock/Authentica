namespace Api.Requests;

/// <summary>
/// Represents a mfa recovery code redeem request.
/// </summary>
public sealed record MultiFactorRecoveryCodeRedeemRequest
{
    /// <summary>
    /// Gets or sets the user email address which the recovery code is used for.
    /// </summary>
    public required string Email { get; set; } = default!;
    /// <summary>
    /// Gets or sets the recovery code.
    /// </summary>
    public required string Code { get; set; } = default!;
}