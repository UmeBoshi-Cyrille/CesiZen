using CesiZen.Application.Services;
using CesiZen.Domain.BusinessResult;
using CesiZen.Domain.Datamodel;
using CesiZen.Domain.DataTransfertObject;
using CesiZen.Domain.Interfaces;
using CesiZen.Infrastructure.Providers;
using CesiZen.Test.Fakers;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using Serilog;

namespace CesiZen.Test.LoginServices;

public class AuthenticateServiceTests
{
    private readonly Mock<IConfiguration> configurationMock;
    private readonly Mock<ISessionQuery> sessionQueryMock;
    private readonly Mock<ISessionCommand> sessionCommandMock;
    private readonly Mock<IRefreshTokenCommand> refreshTokenCommandMock;
    private readonly Mock<IRefreshTokenQuery> refreshTokenQueryMock;
    private readonly Mock<JwtSettings> settingsMock;
    private readonly Mock<ILogger> loggerMock;
    private readonly Mock<IUserCommand> userCommandMock;
    private readonly Mock<IUserQuery> userQueryMock;
    private readonly Mock<IPasswordService> passwordServiceMock;
    private readonly Mock<ILoginQuery> loginQueryMock;
    private readonly Mock<ILoginCommand> loginCommandMock;
    private readonly Mock<ITokenProvider> tokenProviderMock;
    private readonly Mock<IEmailService> emailServiceMock;
    private readonly AuthenticationService authService;

    private readonly TokenProvider tokenProvider;
    private readonly PasswordService passwordService;

    private readonly IServiceCollection services;
    private readonly IServiceProvider serviceProvider;

    public AuthenticateServiceTests()
    {
        configurationMock = new Mock<IConfiguration>();
        sessionQueryMock = new Mock<ISessionQuery>();
        sessionCommandMock = new Mock<ISessionCommand>();
        refreshTokenCommandMock = new Mock<IRefreshTokenCommand>();
        refreshTokenQueryMock = new Mock<IRefreshTokenQuery>();
        settingsMock = new Mock<JwtSettings>();

        loggerMock = new Mock<ILogger>();
        userCommandMock = new Mock<IUserCommand>();
        userQueryMock = new Mock<IUserQuery>();
        passwordServiceMock = new Mock<IPasswordService>();
        loginQueryMock = new Mock<ILoginQuery>();
        loginCommandMock = new Mock<ILoginCommand>();
        tokenProviderMock = new Mock<ITokenProvider>();
        emailServiceMock = new Mock<IEmailService>();

        authService = new AuthenticationService(
            loggerMock.Object,
            userCommandMock.Object,
            userQueryMock.Object,
            passwordServiceMock.Object,
            loginQueryMock.Object,
            loginCommandMock.Object,
            tokenProviderMock.Object,
            emailServiceMock.Object
        );

        tokenProvider = new TokenProvider(
            sessionQueryMock.Object,
            sessionCommandMock.Object,
            refreshTokenCommandMock.Object,
            refreshTokenQueryMock.Object,
            loggerMock.Object,
            LoginFaker.FakeSettings()
            );

        passwordService = new PasswordService(
            LoginFaker.GetConfiguration(),
            loginQueryMock.Object,
            loginCommandMock.Object,
            tokenProviderMock.Object,
            emailServiceMock.Object
            );
    }

    [Fact]
    public async Task AuthenticateTest_ReturnsSuccess_ValidCredentials()
    {
        // Arrange
        var dto = LoginFaker.FakeRequestDtoGenerator().Generate();
        var login = LoginFaker.FakeLoginGenerator().Generate();
        var token = tokenProvider.GenerateAccessToken(login.Id);
        login.Email = dto.Identifier;
        login.Password = passwordService.HashPassword(dto.Password).Password;

        loginQueryMock.Setup(x => x.GetByEmail(It.IsAny<string>())).ReturnsAsync(Result<Login>.Success(login));
        passwordServiceMock.Setup(x => x.VerifyPassword(It.IsAny<Login>(), It.IsAny<string>())).Returns(true);
        tokenProviderMock.Setup(x => x.GenerateAccessToken(It.IsAny<string>())).Returns(token);

        // Act
        var result = await authService.Authenticate(dto);

        // Assert
        Assert.True(result.IsSuccess);
        Assert.True(result.Value.IsLoggedIn);
        Assert.Equal(token, result.Value.Token);
    }

    [Fact]
    public async Task AuthenticateTest_ReturnsFailure_InvalidCredentials()
    {
        // Arrange
        var login = LoginFaker.FakeLoginGenerator().Generate();
        var dto = LoginFaker.FakeRequestDtoGenerator().Generate("wrongPassword123");

        loginQueryMock.Setup(x => x.GetByEmail(It.IsAny<string>())).ReturnsAsync(Result<Login>.Success(login));
        passwordServiceMock.Setup(x => x.VerifyPassword(It.IsAny<Login>(), It.IsAny<string>())).Returns(false);

        // Act
        var result = await authService.Authenticate(dto);

        // Assert
        Assert.True(result.IsFailure);
        Assert.Equal(UserErrors.ClientAuthenticationFailed, result.Error);
    }

    [Fact]
    public async Task VerifyEmail_ValidToken_ReturnsSuccessResult()
    {
        // Arrange
        var email = "test@example.com";
        var token = "validtoken";
        var login = new Login { EmailVerificationToken = token };
        loginQueryMock.Setup(x => x.GetByEmail(email)).ReturnsAsync(Result<Login>.Success(login));
        loginCommandMock.Setup(x => x.UpdateEmailVerification(It.IsAny<EmailVerificationDto>())).ReturnsAsync(Result.Success());

        // Act
        var result = await authService.VerifyEmail(token, email);

        // Assert
        Assert.True(result.IsSuccess);
        Assert.Equal(UserInfos.ClientEmailVerified, result.Info);
    }

    [Fact]
    public async Task VerifyEmail_InvalidToken_ReturnsFailureResult()
    {
        // Arrange
        var email = "test@example.com";
        var token = "invalidtoken";
        var login = new Login { EmailVerificationToken = "differenttoken" };
        loginQueryMock.Setup(x => x.GetByEmail(email)).ReturnsAsync(Result<Login>.Success(login));

        // Act
        var result = await authService.VerifyEmail(token, email);

        // Assert
        Assert.True(result.IsFailure);
        Assert.Equal(UserErrors.ClientEmailVerificationFailed, result.Error);
    }

    [Fact]
    public async Task Disconnect_ValidToken_ReturnsSuccessResult()
    {
        // Arrange
        var accessToken = "validtoken";
        var sessionId = "session";
        var userId = "123";
        tokenProviderMock.Setup(x => x.GetTokenSessionId(accessToken)).Returns(sessionId);
        userQueryMock.Setup(x => x.GetUserId(sessionId)).ReturnsAsync(Result<string>.Success(userId));
        tokenProviderMock.Setup(x => x.InvalidateTokens(userId)).ReturnsAsync(Result.Success());

        // Act
        var result = await authService.Disconnect(accessToken);

        // Assert
        Assert.True(result.IsSuccess);
    }

    // Additional tests for edge cases and private methods can be added here
}
