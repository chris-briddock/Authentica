using Api.Requests;

namespace Application.Activities;

/// <summary>
/// Represents an activity that occurs when a two-factor authentication recovery code is redeemed.
/// </summary>
public class TwoFactorRecoveryCodesRedeemActivity : ActivityBase<TwoFactorRecoveryCodeRedeemRequest>
{
    /// <summary>
    /// Gets or sets the payload containing the request data for redeeming a two-factor authentication recovery code.
    /// </summary>
    public override TwoFactorRecoveryCodeRedeemRequest Payload { get; set; } = default!;
}

