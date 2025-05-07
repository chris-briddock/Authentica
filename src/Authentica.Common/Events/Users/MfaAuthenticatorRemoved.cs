namespace Common.Events;

/// <summary>
/// Event triggered when a mfa authenticator is removed.
/// </summary>
/// <param name="Email">The email address of the user who the authenticator was removed for.</param>
/// <param name="OccurredOn">The date and time when the authenticator was removed.</param>
public sealed record MfaAuthenticatorRemoved(string Email,
                                             DateTime OccurredOn);