using Api.Requests;

namespace Application.Activities;

/// <summary>
/// Represents an activity that occurs when a mfa authentication recovery code is redeemed.
/// </summary>
public class MultiFactorRecoveryCodesRedeemActivity : ActivityBase<MultiFactorRecoveryCodeRedeemRequest>
{
    /// <summary>
    /// Gets or sets the payload containing the request data for redeeming a mfa recovery code.
    /// </summary>
    public override MultiFactorRecoveryCodeRedeemRequest Payload { get; set; } = default!;
}

