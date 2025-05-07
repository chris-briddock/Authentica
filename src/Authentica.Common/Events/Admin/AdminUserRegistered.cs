namespace Common.Events;

/// <summary>
/// Event triggered when an admin is registered.
/// </summary>
/// <param name="Email">The email address of the registered admin.</param>
/// <param name="OccurredOn">The date and time when the admin was registered.</param>
/// <param name="RegisteredBy">The user who registered the admin.</param>

public sealed record AdminUserRegistered(string Email,
                                        DateTime OccurredOn,
                                        string RegisteredBy);