namespace Domain.Events;

/// <summary>
/// Event triggered when an application is updated.
/// </summary>
/// <param name="CurrentName">The current name of the application.</param> 
/// <param name="NewName">The new name of the application.</param>
/// <param name="NewCallbackUri">The new callback uri of the application.</param>
/// <param name="OccurredOn">The date and time when the application was updated.</param>
/// <param name="UpdatedBy">The user who updated the application.</param>

public sealed record ApplicationUpdated(string CurrentName,
                                        string? NewName,
                                        string? NewCallbackUri,
                                        DateTime OccurredOn,
                                        string UpdatedBy);