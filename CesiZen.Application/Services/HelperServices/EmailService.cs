using CesiZen.Domain.Interfaces;
using MailKit.Security;
using Microsoft.Extensions.Configuration;
using MimeKit;
using System.Net.Sockets;
using SmtpClient = MailKit.Net.Smtp.SmtpClient;

namespace CesiZen.Application.Services;

public class EmailService : IEmailService
{
    private readonly IConfiguration configuration;

    public EmailService(IConfiguration configuration)
    {
        this.configuration = configuration;
    }

    public async Task SendVerificationEmailAsync(string email, string verificationToken)
    {
        var message = new MimeMessage();
        message.From.Add(new MailboxAddress("Your App Name", configuration["SMTP:Username"]));
        message.To.Add(new MailboxAddress("", email));
        message.Subject = "Email Verification";

        var verificationLink = $"https://yourapp.com/verify?token={verificationToken}";
        message.Body = new TextPart("plain")
        {
            Text = $"Please verify your email by clicking this link: {verificationLink}"
        };

        using var client = new SmtpClient();

        client.Timeout = 30000; // Set timeout to 30 seconds
        try
        {
            await client.ConnectAsync(configuration["SMTP:Host"], int.Parse(configuration["SMTP:Port"]), SecureSocketOptions.SslOnConnect);
        }
        catch (SocketException ex)
        {
            Console.WriteLine($"Socket error: {ex.SocketErrorCode}, {ex.Message}");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Connection error: {ex.Message}");
        }

        await client.AuthenticateAsync(configuration["SMTP:Username"], configuration["SMTP:Password"]);
        await client.SendAsync(message);
        await client.DisconnectAsync(true);
    }

    public string GenerateVerificationToken()
    {
        return Convert.ToBase64String(Guid.NewGuid().ToByteArray());
    }
}

