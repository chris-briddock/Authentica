namespace Authentica.Common;

/// <summary>
/// Represents a data transfer object for an email message, 
/// which are published to the message queue.
/// </summary>
public sealed class EmailMessage
{
    /// <summary>
    /// The email address to which the email should be sent to.
    /// </summary>
    public string EmailAddress { get; set; } = default!;
    /// <summary>
    /// The type of email to be sent to the message queue. <see cref="EmailPublisherConstants"/> 
    /// </summary>
    public string Type { get; set; } = default!;

    /// <summary>
    /// The code for the user to enter sent via email.
    /// </summary>
    public string Code { get; set; } = default!;
}
