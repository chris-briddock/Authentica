namespace Domain.Events;

/// <summary>
/// Event triggered when a user's email is confirmed.
/// </summary>
/// <param name="Email">The email address of the user whose email was confirmed.</param>
/// <param name="OccurredOn">The date and time when the email was confirmed.</param>
public sealed record UserEmailConfirmed(string Email,
                                        DateTime OccurredOn);