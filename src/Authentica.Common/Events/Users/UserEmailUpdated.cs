namespace Common.Events;

/// <summary>
/// Event triggered when a user's email is updated.
/// </summary>
/// <param name="Email">The email address of the user whose email was updated.</param>
/// <param name="OccurredOn">The date and time when the email was updated.</param>
public sealed record UserEmailUpdated(string Email,
                                      DateTime OccurredOn);