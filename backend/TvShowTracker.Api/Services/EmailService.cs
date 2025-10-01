using SendGrid;
using SendGrid.Helpers.Mail;
using System;
using System.Threading.Tasks;

/// <summary>
/// Defines the contract for sending emails.
/// </summary>
public interface IEmailService
{
    /// <summary>
    /// Sends an email asynchronously with both plain text and HTML content.
    /// </summary>
    /// <param name="to">The recipient email address.</param>
    /// <param name="subject">The subject of the email.</param>
    /// <param name="body">The plain text body of the email.</param>
    /// <param name="htmlBody">The HTML body of the email (optional, falls back to plain text if null).</param>
    Task SendEmailAsync(string to, string subject, string body, string htmlBody);
}

/// <summary>
/// Implementation of <see cref="IEmailService"/> using SendGrid for sending emails.
/// </summary>
public class EmailService : IEmailService
{
    private readonly string _sendGridApiKey;
    private readonly string _fromEmail;
    private readonly string _fromName;

    /// <summary>
    /// Initializes a new instance of the <see cref="EmailService"/> class.
    /// </summary>
    /// <param name="sendGridApiKey">The SendGrid API key.</param>
    /// <param name="fromEmail">The sender email address.</param>
    /// <param name="fromName">The sender display name. Defaults to "No-Reply".</param>
    /// <exception cref="ArgumentNullException">
    /// Thrown when <paramref name="sendGridApiKey"/> or <paramref name="fromEmail"/> is null.
    /// </exception>
    public EmailService(string sendGridApiKey, string fromEmail, string fromName = "No-Reply")
    {
        _sendGridApiKey = sendGridApiKey ?? throw new ArgumentNullException(nameof(sendGridApiKey));
        _fromEmail = fromEmail ?? throw new ArgumentNullException(nameof(fromEmail));
        _fromName = fromName;
    }

    /// <summary>
    /// Sends an email asynchronously using SendGrid.
    /// </summary>
    /// <param name="to">The recipient email address.</param>
    /// <param name="subject">The subject of the email.</param>
    /// <param name="plainTextBody">The plain text content of the email.</param>
    /// <param name="htmlBody">The HTML content of the email (optional, falls back to plain text if null).</param>
    /// <exception cref="ArgumentException">Thrown when <paramref name="to"/> is null or whitespace.</exception>
    /// <exception cref="Exception">Thrown when the SendGrid API request fails.</exception>
    public async Task SendEmailAsync(string to, string subject, string plainTextBody, string htmlBody)
    {
        if (string.IsNullOrWhiteSpace(to))
            throw new ArgumentException("Recipient email is required.", nameof(to));

        var client = new SendGridClient(_sendGridApiKey);
        var from = new EmailAddress(_fromEmail, _fromName);
        var toEmail = new EmailAddress(to);

        var msg = MailHelper.CreateSingleEmail(
            from,
            toEmail,
            subject,
            plainTextContent: plainTextBody,
            htmlContent: htmlBody ?? plainTextBody
        );

        var response = await client.SendEmailAsync(msg);

        if (!response.IsSuccessStatusCode)
        {
            var responseBody = await response.Body.ReadAsStringAsync();
            throw new Exception($"Failed to send email. Status: {response.StatusCode}, Response: {responseBody}");
        }
    }
}
