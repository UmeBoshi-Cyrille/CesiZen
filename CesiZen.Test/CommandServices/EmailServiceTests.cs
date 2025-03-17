using CesiZen.Application.Services;
using MailKit.Net.Smtp;
using Microsoft.Extensions.Configuration;
using MimeKit;
using Moq;

namespace CesiZen.Test.CommandServices;

public class EmailServiceTests
{
    private readonly Mock<ISmtpClient> mockSmtpClient;
    private readonly Mock<IConfiguration> mockConfiguration;
    private readonly EmailService emailService;

    public EmailServiceTests()
    {
        mockConfiguration = new Mock<IConfiguration>();
        mockConfiguration.Setup(x => x["Email:Sender"]).Returns("sender@example.com");
        mockConfiguration.Setup(x => x["Email:SmtpServer"]).Returns("smtp.example.com");
        mockConfiguration.Setup(x => x["Email:Username"]).Returns("username");
        mockConfiguration.Setup(x => x["Email:Password"]).Returns("password");
        mockConfiguration.Setup(x => x["App:Url"]).Returns("https://example.com");

        emailService = new EmailService(mockConfiguration.Object);
        mockSmtpClient = new Mock<ISmtpClient>();
    }

    [Fact]
    public async Task SendVerificationEmailAsync_ShouldSendEmail()
    {
        // Arrange
        string to = "recipient@example.com";
        string subject = "Test Subject";
        string body = "<p>Test Body</p>";
        var email = "test@example.com";
        var verificationToken = "testToken";

        // Act
        await emailService.SendVerificationEmailAsync(email, verificationToken);

        // Assert
        mockSmtpClient.Verify(x => x.ConnectAsync("smtp.example.com", 587, false, default), Times.Once);
        mockSmtpClient.Verify(x => x.AuthenticateAsync("username", "password", default), Times.Once);
        mockSmtpClient.Verify(x => x.SendAsync(It.Is<MimeMessage>(m =>
            m.To.ToString() == to &&
            m.Subject == subject &&
            m.HtmlBody == body), default, default), Times.Once);
        mockSmtpClient.Verify(x => x.DisconnectAsync(true, default), Times.Once);
    }

    [Fact]
    public async Task SendVerificationEmailAsync_ShouldThrowException_WhenSmtpClientFails()
    {
        // Arrange
        mockSmtpClient.Setup(x => x.ConnectAsync(It.IsAny<string>(), It.IsAny<int>(), It.IsAny<bool>(), default))
            .ThrowsAsync(new SmtpCommandException(SmtpErrorCode.MessageNotAccepted, SmtpStatusCode.ErrorInProcessing, "Connection failed"));

        // Act & Assert
        await Assert.ThrowsAsync<SmtpCommandException>(() =>
            emailService.SendVerificationEmailAsync("test@example.com", "Test"));
    }


    [Fact]
    public async Task SendEmailAsync_ShouldThrowException_WhenSmtpClientFails()
    {
        // Arrange
        mockSmtpClient.Setup(x => x.ConnectAsync(It.IsAny<string>(), It.IsAny<int>(), It.IsAny<bool>(), default))
            .ThrowsAsync(new SmtpCommandException(SmtpErrorCode.MessageNotAccepted, SmtpStatusCode.ErrorInProcessing, "Connection failed"));

        // Act & Assert
        await Assert.ThrowsAsync<SmtpCommandException>(() =>
            emailService.SendEmailAsync("test@example.com", "Test", "Test Body"));
    }

    [Fact]
    public async Task SendEmailAsync_ShouldSendEmail()
    {
        // Arrange
        var email = "test@example.com";
        var template = "<html><body>Test</body></html>";
        var subject = "Test Subject";

        // Act
        await emailService.SendEmailAsync(email, template, subject);

        // Assert
        mockSmtpClient.Verify(x => x.ConnectAsync("smtp.example.com", 587, false, default), Times.Once);
        mockSmtpClient.Verify(x => x.AuthenticateAsync("username", "password", default), Times.Once);
        mockSmtpClient.Verify(x => x.SendAsync(It.Is<MimeMessage>(m =>
            m.To.ToString() == email &&
            m.Subject == subject &&
            m.HtmlBody == template), default, default), Times.Once);
        mockSmtpClient.Verify(x => x.DisconnectAsync(true, default), Times.Once);
    }

    [Fact]
    public void ReplaceLinkContent_ShouldReplaceContentCorrectly()
    {
        // Arrange
        var template = "Click here: {{link}}";
        var link = "https://example.com";
        var content = "link";

        // Act
        var result = EmailService.ReplaceLinkContent(template, link, content);

        // Assert
        Assert.Equal("Click here: https://example.com", result);
    }
}
