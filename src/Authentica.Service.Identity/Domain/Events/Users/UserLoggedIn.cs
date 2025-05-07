namespace Domain.Events;

/// <summary>
/// Event triggered when a user logs in.
/// </summary>
/// <param name="Email">The email address of the user who logged in.</param>
/// <param name="OccurredOn">The date and time when the user logged in.</param>

public sealed record UserLoggedIn(string Email,
                                  DateTime OccurredOn);