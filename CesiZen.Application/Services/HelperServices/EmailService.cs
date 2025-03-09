using CesiZen.Domain.BusinessResult;
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
        message.From.Add(new MailboxAddress("CesiZen", configuration["Email:Username"]));
        message.To.Add(new MailboxAddress("", email));
        message.Subject = "Email Verification";

        var verificationLink = $"{configuration["App:Url"]}/verify?token={verificationToken}";
        var template = Message.GetResource("Templates", "VERIFICATION_EMAIL");
        var htmlTemplate = template.Replace("{{url}}", verificationLink);
        message.Body = new TextPart("plain")
        {
            Text = htmlTemplate
        };

        using var client = new SmtpClient();

        client.Timeout = 30000; // Set timeout to 30 seconds
        try
        {
            await client.ConnectAsync(configuration["Email:Host"], 587, SecureSocketOptions.StartTls);
            await client.AuthenticateAsync(configuration["Email:Username"], configuration["Email:Password"]);
            await client.SendAsync(message);
            await client.DisconnectAsync(true);
        }
        catch (SocketException ex)
        {
            Console.WriteLine($"Socket error: {ex.SocketErrorCode}, {ex.Message}");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Connection error: {ex.Message}");
        }
        finally
        {
            client.Dispose();
        }
    }

    public async Task SendEmailAsync(string email, string template, string subject)
    {
        var message = new MimeMessage();
        message.From.Add(new MailboxAddress("Your App Name", configuration["Email:Sender"]));
        message.To.Add(new MailboxAddress("", email));
        message.Subject = subject;

        message.Body = new TextPart("html")
        {
            Text = template
        };

        using var client = new SmtpClient();

        try
        {
            await client.ConnectAsync(configuration["Email:Host"], int.Parse(configuration["Email:Port"]), SecureSocketOptions.StartTls);
            await client.AuthenticateAsync(configuration["Email:Username"], configuration["Email:Password"]);
            await client.SendAsync(message);
            await client.DisconnectAsync(true);
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
        }
        finally
        {
            client.Dispose();
        }
    }

    public string ReplaceLinkContent(string template, string link, string content)
    {
        return template.Replace($"{{{content}}}", link);
    }
}

