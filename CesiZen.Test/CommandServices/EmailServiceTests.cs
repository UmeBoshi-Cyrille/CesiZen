using CesiZen.Application.Services;
using MailKit.Net.Smtp;
using Microsoft.Extensions.Configuration;
using Moq;

namespace CesiZen.Test.CommandServices;

public class EmailServiceTests
{
    private readonly Mock<SmtpClient> mockSmtpClient;
    private readonly Mock<IConfiguration> mockConfiguration;
    private readonly EmailService emailService;

    public EmailServiceTests()
    {
        mockConfiguration = new Mock<IConfiguration>();
        mockConfiguration.Setup(x => x["Email:Host"]).Returns("smtp.example.com");
        mockConfiguration.Setup(x => x["Email:Sender"]).Returns("sender@example.com");
        mockConfiguration.Setup(x => x["Email:SmtpServer"]).Returns("smtp.example.com");
        mockConfiguration.Setup(x => x["Email:Username"]).Returns("username");
        mockConfiguration.Setup(x => x["Email:Password"]).Returns("password");
        mockConfiguration.Setup(x => x["App:Url"]).Returns("https://example.com");

        emailService = new EmailService(mockConfiguration.Object);
        mockSmtpClient = new Mock<SmtpClient>();
    }

    //[Fact]
    //public async Task SendVerificationEmailAsync_ShouldSendEmail()
    //{
    //    // Arrange
    //    string to = "recipient@example.com";
    //    string subject = "Test Subject";
    //    string body = "<p>Test Body</p>";
    //    var email = "test@example.com";
    //    var verificationToken = "testToken";

    //    //mockSmtpClient.Setup(x => x.ConnectAsync(It.IsAny<string>(), It.IsAny<int>(), It.IsAny<bool>(), default))
    //    //.Returns(Task.CompletedTask).Verifiable();
    //    //mockSmtpClient.Setup(x => x.AuthenticateAsync(It.IsAny<string>(), It.IsAny<string>(), default))
    //    //.Returns(Task.CompletedTask).Verifiable();
    //    //mockSmtpClient.Setup(x => x.SendAsync(It.IsAny<MimeMessage>(), default, default))
    //    //    .ReturnsAsync("Success").Verifiable();
    //    //mockSmtpClient.Setup(x => x.DisconnectAsync(It.IsAny<bool>(), default))
    //    //    .Returns(Task.CompletedTask).Verifiable();

    //    // Act
    //    await emailService.SendVerificationEmailAsync(email, verificationToken, mockSmtpClient.Object);

    //    // Assert
    //    mockSmtpClient.Verify(x => x.ConnectAsync(It.IsAny<string>(), It.IsAny<int>(), It.IsAny<bool>(), default), Times.Once);
    //    mockSmtpClient.Verify(x => x.AuthenticateAsync(It.IsAny<string>(), It.IsAny<string>(), default), Times.Once);
    //    mockSmtpClient.Verify(x => x.SendAsync(It.Is<MimeMessage>(m =>
    //        m.To.ToString() == to &&
    //        m.Subject == subject &&
    //        m.HtmlBody == body), default, default), Times.Once);
    //    mockSmtpClient.Verify(x => x.DisconnectAsync(true, default), Times.Once);

    //    mockSmtpClient.VerifyAll();
    //}

    //[Fact]
    //public async Task SendVerificationEmailAsync_ShouldThrowException_WhenSmtpClientFails()
    //{
    //    // Arrange
    //    mockSmtpClient.Setup(x => x.ConnectAsync(It.IsAny<string>(), It.IsAny<int>(), It.IsAny<bool>(), default))
    //        .ThrowsAsync(new SmtpCommandException(SmtpErrorCode.MessageNotAccepted, SmtpStatusCode.ErrorInProcessing, "Connection failed"));

    //    // Act & Assert
    //    await Assert.ThrowsAsync<SmtpCommandException>(() =>
    //        emailService.SendVerificationEmailAsync("test@example.com", "Test"));
    //}


    //[Fact]
    //public async Task SendEmailAsync_ShouldThrowException_WhenSmtpClientFails()
    //{
    //    // Arrange
    //    mockSmtpClient.Setup(x => x.ConnectAsync(It.IsAny<string>(), It.IsAny<int>(), It.IsAny<bool>(), default))
    //        .ThrowsAsync(new SmtpCommandException(SmtpErrorCode.MessageNotAccepted, SmtpStatusCode.ErrorInProcessing, "Connection failed"));

    //    // Act & Assert
    //    await Assert.ThrowsAsync<SmtpCommandException>(() =>
    //        emailService.SendEmailAsync("test@example.com", "Test", "Test Body"));
    //}

    //[Fact]
    //public async Task SendEmailAsync_ShouldSendEmail()
    //{
    //    // Arrange
    //    var email = "test@example.com";
    //    var template = "<html><body>Test</body></html>";
    //    var subject = "Test Subject";

    //    // Act
    //    await emailService.SendEmailAsync(email, template, subject);

    //    // Assert
    //    mockSmtpClient.Verify(x => x.ConnectAsync("smtp.example.com", 587, false, default), Times.Once);
    //    mockSmtpClient.Verify(x => x.AuthenticateAsync("username", "password", default), Times.Once);
    //    mockSmtpClient.Verify(x => x.SendAsync(It.Is<MimeMessage>(m =>
    //        m.To.ToString() == email &&
    //        m.Subject == subject &&
    //        m.HtmlBody == template), default, default), Times.Once);
    //    mockSmtpClient.Verify(x => x.DisconnectAsync(true, default), Times.Once);
    //}
}
