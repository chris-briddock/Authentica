namespace Common.Events;

/// <summary>
/// Event triggered when an application is deleted.
/// </summary>
/// <param name="Name">The name of the deleted application.</param>
/// <param name="OccurredOn">The date and time when the application was deleted.</param>
/// <param name="DeletedBy">The user who deleted the application.</param>

public sealed record ApplicationDeleted(string Name,
                                        DateTime OccurredOn,
                                        string DeletedBy);