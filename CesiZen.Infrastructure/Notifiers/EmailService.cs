using CesiZen.Domain.DataTransfertObject;
using CesiZen.Domain.Interfaces;
using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.Extensions.Configuration;
using MimeKit;

namespace CesiZen.Infrastructure;

public class EmailService : IObserver
{
    private readonly IConfiguration configuration;

    public EmailService(IConfiguration configuration)
    {
        this.configuration = configuration;
    }

    public async void Update(object sender, MessageEventArgs dto)
    {
        await SendEmailAsync(dto.Email, dto.Subject, dto.Body);
    }

    private async Task SendEmailAsync(string email, string subject, string body)
    {
        var message = new MimeMessage();
        message.From.Add(new MailboxAddress("CesiZen", configuration["Email:Sender"]));
        message.To.Add(new MailboxAddress("", email));
        message.Subject = subject;

        message.Body = new TextPart("html")
        {
            Text = body
        };

        using var client = new SmtpClient();

        try
        {
            await client.ConnectAsync(configuration["Email:Host"], int.Parse(configuration["Email:Port"]!), SecureSocketOptions.StartTls);
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

    public static string ReplaceLinkContent(string template, string link, string content)
    {
        return template.Replace($"{{{content}}}", link);
    }
}
