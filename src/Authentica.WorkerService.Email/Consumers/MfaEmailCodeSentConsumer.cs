using Authentica.WorkerService.Email.Services;
using Common.Events;
using MassTransit;

namespace Authentica.WorkerService.Email.Consumers;

/// <summary>
/// Consumer for handling MfaEmailCodeSent events.
/// </summary>
public class MfaEmailCodeSentConsumer : IEmailConsumer<MfaEmailCodeSent>
{
    private readonly EmailService _emailService;
    private readonly ILogger<MfaEmailCodeSentConsumer> _logger;

    /// <summary>
    /// Initializes a new instance of the <see cref="MfaEmailCodeSentConsumer"/> class.
    /// </summary>
    /// <param name="emailService">The email service.</param>
    /// <param name="logger">The logger.</param>
    public MfaEmailCodeSentConsumer(
        EmailService emailService,
        ILogger<MfaEmailCodeSentConsumer> logger)
    {
        _emailService = emailService ?? throw new ArgumentNullException(nameof(emailService));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    /// <summary>
    /// Consumes the MfaEmailCodeSent event and sends an MFA code email.
    /// </summary>
    /// <param name="context">The consume context containing the event.</param>
    /// <returns>A task representing the asynchronous operation.</returns>
    public async Task Consume(ConsumeContext<MfaEmailCodeSent> context)
    {
        try
        {
            _logger.LogInformation("Consuming MfaEmailCodeSent event for {Email}", context.Message.Email);
            
            var subject = "Your Multi-Factor Authentication Code";
            var emailBody = _emailService.CreateEmailTemplate(
                "MFA Verification Code",
                "MFA Verification Code",
                context.Message.Email,
                $"Your MFA verification code is <span class=\"font-bold text-indigo-800\">{context.Message.Code}</span>",
                "This code will expire shortly. Please use it immediately to complete your login."
            );

            await _emailService.SendEmailAsync(context.Message.Email, subject, emailBody);
            
            _logger.LogInformation("MFA email code sent successfully to {Email}", context.Message.Email);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error processing MfaEmailCodeSent event for {Email}", context.Message.Email);
            throw;
        }
    }
}
