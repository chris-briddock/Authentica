namespace Application.DTOs;

/// <summary>
/// Represents a data transfer object (DTO) for a user session.
/// This class is used to transfer essential session data between layers
/// while excluding unnecessary details for improved performance and clarity.
/// </summary>
public class SessionDto
{
    /// <summary>
    /// Gets or sets the current status of the session.
    /// </summary>
    /// <remarks>
    /// Indicates the state of the session, such as "Active", "Expired", or "Terminated".
    /// </remarks>
    public required string Status { get; set; } = default!;

    /// <summary>
    /// Gets or sets the user ID associated with this session.
    /// </summary>
    /// <remarks>
    /// This property identifies the user to whom the session belongs.
    /// </remarks>
    public required string UserId { get; set; } = default!;

    /// <summary>
    /// Gets or sets the start time of the session.
    /// </summary>
    /// <remarks>
    /// This value is set when the session begins and can be used to calculate session duration.
    /// </remarks>
    public required DateTime StartDateTime { get; set; }

    /// <summary>
    /// Gets or sets the end time of the session, if applicable.
    /// </summary>
    /// <remarks>
    /// This property is null if the session is still active. Otherwise, it indicates when the session ended.
    /// </remarks>
    public required DateTime? EndDateTime { get; set; }

    /// <summary>
    /// Gets or sets the user agent string of the client that initiated the session.
    /// </summary>
    /// <remarks>
    /// The user agent string typically contains information about the client's browser, operating system, 
    /// and device. This data can be used for analytics, debugging, or tailoring the user experience.
    /// It provides insights into the types of devices and platforms interacting with the system, which can 
    /// help in optimizing for specific environments or detecting potentially malicious access patterns.
    /// </remarks>
    public required string UserAgent { get; set; } = default!;

    /// <summary>
    /// Gets or sets the IP address from which the session originated.
    /// </summary>
    /// <remarks>
    /// The IP address can be used for various purposes, such as:
    /// - **Security Auditing**: Tracking the origin of sessions to identify suspicious activities.
    /// - **Geographical Analysis**: Determining the approximate location of the user.
    /// - **Access Control**: Restricting or allowing access based on IP ranges.
    /// This property is required to ensure that each session has an associated source address, 
    /// but it should be handled with care to respect user privacy.
    /// </remarks>
    public required string? IpAddress { get; set; } = default!;
}