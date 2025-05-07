namespace Domain.Events;

/// <summary>
/// Event triggered when a passkey is verified.
/// </summary>
/// <param name="Email">The email address of the user who the passkey was verified for.</param>
/// <param name="OccurredOn">The date and time when the passkey was verified.</param>
public sealed record PasskeyVerified(string Email,
                                        DateTime OccurredOn);