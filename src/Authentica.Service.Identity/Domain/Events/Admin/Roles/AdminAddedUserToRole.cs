namespace Domain.Events;

/// <summary>
/// Event triggered when a role is created.
/// </summary>
/// <param name="Name">The name of the created role.</param>
/// <param name="OccurredOn">The date and time when the role was created.</param>
/// <param name="CreatedBy">The user who created the role.</param>
public sealed record AdminAddedUserToRole(string Name,
                                          DateTime OccurredOn,
                                          string CreatedBy);