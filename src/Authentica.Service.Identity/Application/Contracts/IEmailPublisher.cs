using Authentica.Common;

namespace Application.Contracts;

/// <summary>
/// Publishes a message to the message queue, for confirmation emails, 
/// password reset codes, and two factor codes, update email codes, 
/// and update phone number codes
/// </summary>
public interface IEmailPublisher
{
    /// <summary>
    /// Publishes a message to the message queue.
    /// </summary>
    /// <param name="emailMessage">The object which encapsulates the email message.</param>
    /// <param name="cancellationToken">The cancellation token which propigates notification that the operation will be cancelled.</param>
    /// <returns>An asyncronous operation of type <see cref="Task"/></returns>
    Task Publish(EmailMessage emailMessage, CancellationToken cancellationToken);
}
