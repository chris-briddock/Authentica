namespace Common.Events;

/// <summary>
/// Event triggered when a user's email confirmation code is sent.
/// </summary>
/// <param name="Email">The email address of the user who the code was sent to.</param>
/// <param name="Code">The code to be send via email.</param>
/// <param name="OccurredOn">The date and time when the code was sent.</param>
public sealed record UserEmailConfirmationCodeSent(string Email,
                                                  string Code,
                                                  DateTime OccurredOn);