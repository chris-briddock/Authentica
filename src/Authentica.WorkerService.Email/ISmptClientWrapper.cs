using System.Net.Mail;

namespace Authentica.WorkerService.Email;

public interface ISmtpClient : IDisposable
{
    Task SendMailAsync(MailMessage message);
}
