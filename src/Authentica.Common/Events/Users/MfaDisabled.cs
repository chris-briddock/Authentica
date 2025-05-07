namespace Common.Events;

/// <summary>
/// Event triggered when mfa is disabled.
/// </summary>
/// <param name="Email">The email address of the user which mfa was disabled.</param>
/// <param name="OccurredOn">The date and time when mfa was disabled.</param>
public sealed record MfaDisabled(string Email,
                                 DateTime OccurredOn);