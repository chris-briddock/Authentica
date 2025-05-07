namespace Common.Events;

/// <summary>
/// Event triggered when a mfa authenticator is registered.
/// </summary>
/// <param name="Email">The email address of the user who the authenticator was registered for.</param>
/// <param name="OccurredOn">The date and time when the authenticator was registered.</param>
public sealed record MfaAuthenticatorRegistered(string Email,
                                                DateTime OccurredOn);