using System.Net;
using System.Net.Mail;

namespace Authentica.WorkerService.Email;

/// <summary>
/// Wrapper class for the SmtpClient to send emails.
/// </summary>
public class SmtpClientWrapper : ISmtpClient
{
    private readonly SmtpClient _smtpClient;

    /// <summary>
    /// Configuration interface for accessing email settings.
    /// </summary>
    public IConfiguration Configuration { get; }

    /// <summary>
    /// Initializes a new instance of the <see cref="SmtpClientWrapper"/> class.
    /// </summary>
    /// <param name="smtpClient">The SmtpClient instance to be used for sending emails.</param>
    public SmtpClientWrapper(SmtpClient smtpClient, IConfiguration configuration)
    {
        _smtpClient = smtpClient;
        Configuration = configuration;
    }

    /// <summary>
    /// Sends an email asynchronously.
    /// </summary>
    /// <param name="message">The email message to be sent.</param>
    /// <returns>A task representing the asynchronous email sending operation.</returns>
    public Task SendMailAsync(MailMessage message)
    {
        var smtpServer = Environment.GetEnvironmentVariable("EMAIL_SERVER")!;
        int smtpPort = Convert.ToInt16(Environment.GetEnvironmentVariable("EMAIL_PORT"));
        var smtpUsername = Environment.GetEnvironmentVariable("EMAIL_CREDENTIALS_EMAIL_ADDRESS");
        var smtpPassword = Environment.GetEnvironmentVariable("EMAIL_CREDENTIALS_PASSWORD");
        _smtpClient.Host = smtpServer;
        _smtpClient.EnableSsl = true;
        _smtpClient.Credentials = new NetworkCredential(smtpUsername, smtpPassword);
        _smtpClient.Port = smtpPort;

        return _smtpClient.SendMailAsync(message);
    }

    /// <summary>
    /// Disposes the SmtpClient instance.
    /// </summary>
    public void Dispose()
    {
        _smtpClient.Dispose();
        GC.SuppressFinalize(this);
    }
}