namespace Common.Events;

/// <summary>
/// Event triggered when a mfa recovery code is redeemed.
/// </summary>
/// <param name="Email">The email address of the user who the recovery code was redeemed for.</param>
/// <param name="OccurredOn">The date and time when the recovery code was redeemed.</param>
public sealed record MfaRecoveryCodeRedeemed(string Email,
                                             DateTime OccurredOn);