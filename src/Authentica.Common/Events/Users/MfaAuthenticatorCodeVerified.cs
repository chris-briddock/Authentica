namespace Common.Events;

/// <summary>
/// Event triggered when a mfa authenticator code is verified.
/// </summary>
/// <param name="Email">The email address of the user who the code was verified for.</param>
/// <param name="OccurredOn">The date and time when the code was verified.</param>
public sealed record MfaAuthenticatorCodeVerified(string Email,
                                                  DateTime OccurredOn);