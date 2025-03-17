namespace CesiZen.Domain.Interfaces;

public interface IEmailService
{
    Task SendVerificationEmailAsync(string email, string verificationToken);

    Task SendEmailAsync(string email, string template, string subject);
}
