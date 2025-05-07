namespace Common.Events;

/// <summary>
/// Event triggered when a user is registered.
/// </summary>
/// <param name="Email">The email address of the registered user.</param>
/// <param name="OccurredOn">The date and time when the user was registered.</param>
public sealed record UserRegistered(string Email,
                                    DateTime OccurredOn);