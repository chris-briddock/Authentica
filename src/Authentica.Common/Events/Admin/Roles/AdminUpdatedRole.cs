namespace Common.Events;

/// <summary>
/// Event triggered when a role is updated.
/// </summary>
/// <param name="CurrentName">The current name of the role.</param> 
/// <param name="NewName">The new name of the role.</param>
/// <param name="OccurredOn">The date and time when the role was updated.</param>
/// <param name="UpdatedBy">The user who updated the role.</param>

public sealed record AdminUpdatedRole(string CurrentName,
                                      string NewName,
                                      DateTime OccurredOn,
                                      string UpdatedBy);