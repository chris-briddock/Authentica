using Api.Requests;

namespace Domain.Events;

/// <summary>
/// Represents an event that occurs when a two-factor authentication recovery code is redeemed.
/// </summary>
public class TwoFactorRecoveryCodesRedeemEvent : EventBase<TwoFactorRecoveryCodeRedeemRequest>
{
    /// <summary>
    /// Gets or sets the payload containing the request data for redeeming a two-factor authentication recovery code.
    /// </summary>
    public override TwoFactorRecoveryCodeRedeemRequest Payload { get; set; } = default!;
}

