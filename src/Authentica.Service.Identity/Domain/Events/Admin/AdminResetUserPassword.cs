namespace Domain.Events;

/// <summary>
/// Event triggered when a user's password is reset by an administrator.
/// </summary>
/// <param name="Email">The email address of the user whose password was reset.</param>
/// <param name="OccurredOn">The date and time when the password was reset.</param>
/// <param name="ResetBy">The user who reset the password.</param>

public sealed record AdminResetUserPassword(string Email,
                                            DateTime OccurredOn,
                                            string ResetBy);