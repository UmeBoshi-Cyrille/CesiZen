namespace CesiZen.Domain.Interfaces;

public interface IEmailService
{
    Task SendVerificationEmailAsync(string email, string verificationToken);

    string GenerateVerificationToken();
}
