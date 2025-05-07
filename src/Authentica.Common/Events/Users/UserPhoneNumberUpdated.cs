namespace Common.Events;

/// <summary>
/// Event triggered when a user's phone number is updated.
/// </summary>
/// <param name="Email">The email address of the user whose phone number was updated.</param>
/// <param name="OccurredOn">The date and time when the phone number was updated.</param>
public sealed record UserPhoneNumberUpdated(string Email,
                                            DateTime OccurredOn);