namespace Domain.Events;

/// <summary>
/// Event triggered when an application is created.
/// </summary>
/// <param name="Name">The name of the created application.</param>
/// <param name="OccurredOn">The date and time when the application was created.</param>
/// <param name="CreatedBy">The user who created the application.</param>

public sealed record ApplicationCreated(string Name,
                                        DateTime OccurredOn,
                                        string CreatedBy);