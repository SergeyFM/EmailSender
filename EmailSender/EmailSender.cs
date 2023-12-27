using System.Net;
using System.Net.Mail;
using App.EmailSender.Contracts;

namespace App.EmailSender;

public class EmailSender : IEmailSender {

    private readonly SmtpConfiguration _smtpConfiguration;
    private readonly ILogger _logger;
    private readonly IConfiguration _configuration;

    public EmailSender(ILogger<EmailSender> logger, IConfiguration configuration) {
        _logger = logger;
        _configuration = configuration;
        _smtpConfiguration = ReadConfigFromAppSettings(configuration);
    }

    private SmtpConfiguration ReadConfigFromAppSettings(IConfiguration configuration) {
        // read from configuration and return SmtpConfiguration
        return new SmtpConfiguration() {
            SmtpServer = configuration["SmtpConfiguration:SmtpServer"],
            Port = int.Parse(configuration["SmtpConfiguration:Port"]),
            EnableSsl = bool.Parse(configuration["SmtpConfiguration:EnableSsl"]),
            From = configuration["SmtpConfiguration:From"],
            UserName = configuration["SmtpConfiguration:UserName"],
            Password = configuration["SmtpConfiguration:Password"]
        };

    }
    public async Task SendEmailAsync(string to, string subject, string body) {
        try {
            MailAddress fromAddr = new(_smtpConfiguration.From);
            MailAddress toAddr = new(to);
            MailMessage message = new(fromAddr, toAddr);
            message.Body = body;
            message.Subject = subject;
            using SmtpClient client = new();
            client.Host = _smtpConfiguration.SmtpServer;
            client.Port = _smtpConfiguration.Port;
            client.EnableSsl = _smtpConfiguration.EnableSsl;
            client.Credentials = new NetworkCredential(_smtpConfiguration.UserName, _smtpConfiguration.Password);
            await client.SendMailAsync(message);
            _logger.LogInformation($"Email sent to {to}");
        }
        catch (Exception ex) {
            _logger.LogError(ex, $"Error sending email to {to}");
        }
    }

}
