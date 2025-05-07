namespace Domain.Events;

/// <summary>
/// Event triggered when mfa is enabled.
/// </summary>
/// <param name="Email">The email address of the user which mfa was enabled.</param>
/// <param name="OccurredOn">The date and time when mfa was enabled.</param>
public sealed record MfaEnabled(string Email,
                                DateTime OccurredOn);