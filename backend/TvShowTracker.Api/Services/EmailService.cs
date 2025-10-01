using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;

/// <summary>
/// Defines the contract for sending emails asynchronously.
/// </summary>
public interface IEmailService
{
    /// <summary>
    /// Sends an email asynchronously.
    /// </summary>
    /// <param name="to">The recipient's email address.</param>
    /// <param name="subject">The subject line of the email.</param>
    /// <param name="body">The body content of the email.</param>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    Task SendEmailAsync(string to, string subject, string body);
}

/// <summary>
/// A basic implementation of <see cref="IEmailService"/> that logs emails to the console.
/// Replace with real SMTP, SendGrid, or other email provider for production use.
/// </summary>
public class EmailService : IEmailService
{
    private readonly ILogger<EmailService> _logger;

    /// <summary>
    /// Initializes a new instance of the <see cref="EmailService"/> class.
    /// </summary>
    /// <param name="logger">The logger used to log email sending actions.</param>
    public EmailService(ILogger<EmailService> logger)
    {
        _logger = logger;
    }

    /// <summary>
    /// Sends an email asynchronously. Currently logs the email details to the console.
    /// </summary>
    /// <param name="to">The recipient's email address.</param>
    /// <param name="subject">The subject line of the email.</param>
    /// <param name="body">The body content of the email.</param>
    /// <returns>A completed <see cref="Task"/>.</returns>
    public Task SendEmailAsync(string to, string subject, string body)
    {
        // Replace this with SMTP/SendGrid/etc
        Console.WriteLine("Sending email to {0} with subject {1} and body {2}", to, subject, body);

        return Task.CompletedTask;
    }
}
