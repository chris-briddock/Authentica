namespace Common.Events;

/// <summary>
/// Event triggered when a user's address is updated.
/// </summary>
/// <param name="Email">The email address of the user whose address was updated.</param>
/// <param name="OccurredOn">The date and time when the address was updated.</param>
public sealed record UserAddressUpdated(string Email,
                                        DateTime OccurredOn);