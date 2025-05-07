namespace Common.Events;

/// <summary>
/// Event triggered when mfa recovery codes are generated.
/// </summary>
/// <param name="Email">The email address of the user who the recovery codes were generated for.</param>
/// <param name="OccurredOn">The date and time when the recovery codes were generated.</param>
public sealed record MfaRecoveryCodesGenerated(string Email,
                                              DateTime OccurredOn);