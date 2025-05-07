namespace Common.Events;

/// <summary>
/// Event triggered when a user is deleted.
/// </summary>
/// <param name="Email">The email address of the deleted user.</param>
/// <param name="OccurredOn">The date and time when the user was deleted.</param>
public sealed record UserDeleted(string Email,
                                 DateTime OccurredOn);