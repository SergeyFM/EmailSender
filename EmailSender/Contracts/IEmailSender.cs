namespace App.EmailSender.Contracts;

public interface IEmailSender {
    Task SendEmailAsync(string to, string subject, string body);
}