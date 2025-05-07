namespace Common.Events;

/// <summary>
/// Event triggered when a role is deleted.
/// </summary>
/// <param name="Name">The name of the deleted role.</param>
/// <param name="OccurredOn">The date and time when the role was deleted.</param>
/// <param name="DeletedBy">The user who deleted the role.</param>

public sealed record AdminDeletedRole(string Name,
                                      DateTime OccurredOn,
                                      string DeletedBy);