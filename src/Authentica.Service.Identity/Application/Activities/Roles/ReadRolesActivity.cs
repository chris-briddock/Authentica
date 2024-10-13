namespace Application.Activities;

/// <summary>
/// Represents an activity that occurs when a group is read or retrieved.
/// </summary>
public sealed class ReadRolesActivity
{
    /// <summary>
    /// Gets or sets the email associated with the group read activity.
    /// </summary>
    public string Email { get; set; } = default!;
}
