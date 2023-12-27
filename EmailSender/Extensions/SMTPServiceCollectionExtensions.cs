using App.EmailSender.Contracts;

namespace App.EmailSender.Extensions;

public static class SMTPServiceCollectionExtensions {
    public static IServiceCollection AddEmailSender(this IServiceCollection services) {
        services.AddTransient<IEmailSender, EmailSender>();
        return services;
    }
}
