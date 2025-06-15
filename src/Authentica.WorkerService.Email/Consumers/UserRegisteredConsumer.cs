using Authentica.WorkerService.Email.Services;
using Common.Events;
using MassTransit;

namespace Authentica.WorkerService.Email.Consumers;

/// <summary>
/// Consumer for handling UserRegistered events.
/// </summary>
public class UserRegisteredConsumer : IEmailConsumer<UserRegistered>
{
    private readonly EmailService _emailService;
    private readonly ILogger<UserRegisteredConsumer> _logger;

    /// <summary>
    /// Initializes a new instance of the <see cref="UserRegisteredConsumer"/> class.
    /// </summary>
    /// <param name="emailService">The email service.</param>
    /// <param name="logger">The logger.</param>
    public UserRegisteredConsumer(
        EmailService emailService,
        ILogger<UserRegisteredConsumer> logger)
    {
        _emailService = emailService ?? throw new ArgumentNullException(nameof(emailService));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    /// <summary>
    /// Consumes the UserRegistered event and sends a welcome email.
    /// </summary>
    /// <param name="context">The consume context containing the event.</param>
    /// <returns>A task representing the asynchronous operation.</returns>
    public async Task Consume(ConsumeContext<UserRegistered> context)
    {
        try
        {
            _logger.LogInformation("Consuming UserRegistered event for {Email}", context.Message.Email);
            
            var subject = "Welcome to Authentica";
            var emailBody = _emailService.CreateEmailTemplate(
                "Welcome to Authentica",
                "Welcome to Authentica",
                context.Message.Email,
                "Thank you for registering with Authentica. Your account has been created successfully.",
                "Please check your inbox for a separate email to confirm your email address."
            );

            await _emailService.SendEmailAsync(context.Message.Email, subject, emailBody);
            
            _logger.LogInformation("Welcome email sent successfully to {Email}", context.Message.Email);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error processing UserRegistered event for {Email}", context.Message.Email);
            throw;
        }
    }
}
