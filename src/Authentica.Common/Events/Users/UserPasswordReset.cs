namespace Common.Events;

/// <summary>
/// Event triggered when a user's password is reset.
/// </summary>
/// <param name="Email">The email address of the user whose password was reset.</param>
/// <param name="OccurredOn">The date and time when the password was reset.</param>
public sealed record UserPasswordReset(string Email,
                                       DateTime OccurredOn);