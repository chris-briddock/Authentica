using System.Net.Mail;

namespace Authentica.WorkerService.Email.Services;

/// <summary>
/// Service for sending emails with common functionality.
/// </summary>
public class EmailService
{
    private readonly ISmtpClient _smtpClient;
    private readonly IConfiguration _configuration;
    private readonly ILogger<EmailService> _logger;

    /// <summary>
    /// Initializes a new instance of the <see cref="EmailService"/> class.
    /// </summary>
    /// <param name="smtpClient">The SMTP client for sending emails.</param>
    /// <param name="configuration">The application configuration.</param>
    /// <param name="logger">The logger.</param>
    public EmailService(
        ISmtpClient smtpClient,
        IConfiguration configuration,
        ILogger<EmailService> logger)
    {
        _smtpClient = smtpClient ?? throw new ArgumentNullException(nameof(smtpClient));
        _configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    /// <summary>
    /// Sends an email with the specified subject and HTML body.
    /// </summary>
    /// <param name="to">The recipient's email address.</param>
    /// <param name="subject">The email subject.</param>
    /// <param name="htmlBody">The HTML body of the email.</param>
    /// <returns>A task representing the asynchronous operation.</returns>
    public async Task SendEmailAsync(string to, string subject, string htmlBody)
    {
        try
        {
            var from = _configuration["Email:Credentials:EmailAddress"]!;
            var message = new MailMessage(from, to)
            {
                Subject = subject,
                Body = htmlBody,
                IsBodyHtml = true
            };

            await _smtpClient.SendMailAsync(message);
            _logger.LogInformation("Email sent successfully to {EmailAddress} with subject: {Subject}", to, subject);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to send email to {EmailAddress} with subject: {Subject}", to, subject);
            throw;
        }
    }

    /// <summary>
    /// Creates a standard email template with the provided content.
    /// </summary>
    /// <param name="title">The title to display in the email.</param>
    /// <param name="heading">The main heading of the email.</param>
    /// <param name="emailAddress">The recipient's email address to display in the greeting.</param>
    /// <param name="mainContent">The main content of the email.</param>
    /// <param name="additionalContent">Optional additional content to display.</param>
    /// <returns>A string containing the HTML email template.</returns>
    public string CreateEmailTemplate(string title,
                                      string heading,
                                      string emailAddress,
                                      string mainContent,
                                      string? additionalContent = null)
    {
        return $@"<!DOCTYPE html>
<html lang=""en"">
<head>
    <meta charset=""UTF-8"">
    <meta name=""viewport"" content=""width=device-width, initial-scale=1.0"">
    <title>{title}</title>
    <link href=""https://cdn.jsdelivr.net/npm/tailwindcss@4.1.5/dist/tailwind.min.css"" rel=""stylesheet"">
</head>
<body class=""font-sans bg-gray-100"">
    <div class=""max-w-screen-md mx-auto p-8 bg-white shadow-md rounded-md"">
        <h2 class=""text-2xl font-semibold mb-4 text-gray-800"">{heading}</h2>
        <p class=""text-gray-700"">Dear <span class=""font-bold text-indigo-800"">{emailAddress}</span>,</p>
        <p class=""text-gray-700"">{mainContent}</p>
        {(additionalContent != null ? $"<p class=\"text-gray-700\">{additionalContent}</p>" : "")}
        <p class=""text-gray-700"">If you did not request this or have any concerns, please contact our support team.</p>
        <p class=""mt-4 text-gray-700"">Thank you,<br>Authentica Team</p>
        <p class=""mt-2 text-gray-600"">© {DateTime.Now.Year} All rights reserved.</p>
    </div>
</body>
</html>";
    }
}
