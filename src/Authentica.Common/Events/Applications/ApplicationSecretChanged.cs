namespace Common.Events;

/// <summary>
/// Event triggered when a secret is created for an application.
/// </summary>
/// <param name="Name">The name of the application.</param>
/// <param name="OccurredOn">The date and time when the secret was created.</param>
/// <param name="CreatedBy">The user who created the secret.</param>

public sealed record ApplicationSecretChanged(string Name,
                                              DateTime OccurredOn,
                                              string CreatedBy);