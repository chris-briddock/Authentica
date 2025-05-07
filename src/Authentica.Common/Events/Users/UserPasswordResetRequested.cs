namespace Common.Events;

/// <summary>
/// Event triggered when a user requests a password reset.
/// </summary>
/// <param name="Email">The email address of the user who requested the password reset.</param>
/// <param name="Code">The code to be send via email.</param>
/// <param name="OccurredOn">The date and time when the password reset was requested.</param>
public sealed record UserPasswordResetRequested(string Email,
                                                string Code,
                                                DateTime OccurredOn);