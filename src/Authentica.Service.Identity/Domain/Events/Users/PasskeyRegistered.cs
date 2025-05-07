namespace Domain.Events;

/// <summary>
/// Event triggered when a passkey is registered.
/// </summary>
/// <param name="Email">The email address of the user who the passkey was registered for.</param>
/// <param name="OccurredOn">The date and time when the passkey was registered.</param>
public sealed record PasskeyRegistered(string Email,
                                        DateTime OccurredOn);