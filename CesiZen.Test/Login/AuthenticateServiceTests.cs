using CesiZen.Application.Services;
using CesiZen.Domain.BusinessResult;
using CesiZen.Domain.Datamodel;
using CesiZen.Domain.DataTransfertObject;
using CesiZen.Domain.Interfaces;
using CesiZen.Domain.Mapper;
using CesiZen.Infrastructure.Providers;
using CesiZen.Test.Fakers;
using Moq;
using Serilog;

namespace CesiZen.Test.LoginServices;

public class AuthenticateServiceTests
{
    private readonly Mock<ISessionQuery> sessionQueryMock;
    private readonly Mock<ISessionCommand> sessionCommandMock;
    private readonly Mock<IRefreshTokenCommand> refreshTokenCommandMock;
    private readonly Mock<IRefreshTokenQuery> refreshTokenQueryMock;
    private readonly Mock<ILogger> loggerMock;
    private readonly Mock<IUserCommand> userCommandMock;
    private readonly Mock<IUserQuery> userQueryMock;
    private readonly Mock<IPasswordService> passwordServiceMock;
    private readonly Mock<ILoginQuery> loginQueryMock;
    private readonly Mock<ILoginCommand> loginCommandMock;
    private readonly Mock<ITokenProvider> tokenProviderMock;
    private readonly Mock<IEmailService> emailServiceMock;
    private readonly AuthenticationService authenticationService;

    private readonly TokenProvider tokenProvider;
    private readonly PasswordService passwordService;

    public AuthenticateServiceTests()
    {
        sessionQueryMock = new Mock<ISessionQuery>();
        sessionCommandMock = new Mock<ISessionCommand>();
        refreshTokenCommandMock = new Mock<IRefreshTokenCommand>();
        refreshTokenQueryMock = new Mock<IRefreshTokenQuery>();

        loggerMock = new Mock<ILogger>();
        userCommandMock = new Mock<IUserCommand>();
        userQueryMock = new Mock<IUserQuery>();
        passwordServiceMock = new Mock<IPasswordService>();
        loginQueryMock = new Mock<ILoginQuery>();
        loginCommandMock = new Mock<ILoginCommand>();
        tokenProviderMock = new Mock<ITokenProvider>();
        emailServiceMock = new Mock<IEmailService>();

        authenticationService = new AuthenticationService(
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
            tokenProviderMock.Object
            );
    }

    [Fact]
    public async Task AuthenticateTest_ReturnsSuccess_ValidCredentials()
    {
        // Arrange
        var dto = LoginFaker.FakeRequestDtoGenerator().Generate();
        var login = LoginFaker.FakeLoginGenerator().Generate();
        var user = UserFaker.FakeAuthenticationUserDtoGenerator().Generate();
        user.Login = login.MapAuthenticationLoginDto();
        var tokenDto = tokenProvider.GenerateRefreshToken(login.UserId);
        var token = tokenProvider.GenerateAccessToken(tokenDto.Value);
        login.Email = dto.Identifier;
        login.Password = passwordService.HashPassword(dto.Password).Password;

        loginQueryMock.Setup(x => x.GetByEmail(It.IsAny<string>())).ReturnsAsync(Result<Login>.Success(login));
        userQueryMock.Setup(x => x.GetByIdentifier(It.IsAny<string>(), It.IsAny<bool>())).ReturnsAsync(Result<AuthenticationUserDto>.Success(user));
        passwordServiceMock.Setup(x => x.IsCorrectPassword(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>())).Returns(true);
        tokenProviderMock.Setup(x => x.GenerateRefreshToken(It.IsAny<int>())).Returns(Result<TokenBuilderDto>.Success(tokenDto.Value));
        tokenProviderMock.Setup(x => x.GenerateAccessToken(It.IsAny<TokenBuilderDto>())).Returns(token);

        // Act
        var result = await authenticationService.Authenticate(dto);

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
        var user = UserFaker.FakeAuthenticationUserDtoGenerator().Generate();
        user.Login = login.MapAuthenticationLoginDto();
        var dto = login.Map();
        dto.Password = "wrongPassword";

        loginQueryMock.Setup(x => x.GetByEmail(It.IsAny<string>())).ReturnsAsync(Result<Login>.Success(login));
        userQueryMock.Setup(x => x.GetByIdentifier(It.IsAny<string>(), It.IsAny<bool>())).ReturnsAsync(Result<AuthenticationUserDto>.Success(user));
        passwordServiceMock.Setup(x => x.IsCorrectPassword(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>())).Returns(false);

        // Act
        var result = await authenticationService.Authenticate(dto);

        // Assert
        Assert.True(result.IsFailure);
        Assert.Equal(UserErrors.ClientAuthenticationFailed.Message, result.Error.Message);
    }

    [Fact]
    public async Task VerifyEmailTest_ReturnsSuccess_ValidToken()
    {
        // Arrange
        var login = LoginFaker.FakeLoginGenerator().Generate();
        var token = tokenProvider.GenerateVerificationToken();
        login.EmailVerificationToken = token;
        loginQueryMock.Setup(x => x.GetByEmail(login.Email)).ReturnsAsync(Result<Login>.Success(login));
        loginCommandMock.Setup(x => x.UpdateEmailVerification(It.IsAny<EmailVerificationDto>())).ReturnsAsync(Result.Success());

        // Act
        var result = await authenticationService.VerifyEmail(token, login.Email);

        // Assert
        Assert.True(result.IsSuccess);
        Assert.Equal(UserInfos.ClientEmailVerified.Message, result.Info.Message);
    }

    [Fact]
    public async Task VerifyEmailTest_InvalidToken_ReturnsFailure()
    {
        // Arrange
        var login = LoginFaker.FakeLoginGenerator().Generate();
        var token = tokenProvider.GenerateVerificationToken();
        loginQueryMock.Setup(x => x.GetByEmail(login.Email)).ReturnsAsync(Result<Login>.Success(login));

        // Act
        var result = await authenticationService.VerifyEmail(token, login.Email);

        // Assert
        Assert.True(result.IsFailure);
        Assert.Equal(UserErrors.ClientEmailVerificationFailed.Message, result.Error.Message);
    }

    [Fact]
    public async Task VerifyEmailTest_LoginNotFound_ReturnsFailure()
    {
        // Arrange
        var email = "fake@email.com";
        var token = tokenProvider.GenerateVerificationToken();
        loginQueryMock.Setup(x => x.GetByEmail(It.IsAny<string>()))
            .ReturnsAsync(Result<Login>.Failure(UserErrors.ClientNotFound));

        // Act
        var result = await authenticationService.VerifyEmail(token, email);

        // Assert
        Assert.True(result.IsFailure);
        Assert.Equal(UserErrors.ClientNotFound.Message, result.Error.Message);
    }

    [Fact]
    public async Task VerifyEmailTest_UpdateEmailVerificationFailed_ReturnsFailure()
    {
        // Arrange
        var login = LoginFaker.FakeLoginGenerator().Generate();
        var token = tokenProvider.GenerateVerificationToken();
        login.EmailVerificationToken = token;
        loginQueryMock.Setup(x => x.GetByEmail(login.Email)).ReturnsAsync(Result<Login>.Success(login));
        loginCommandMock.Setup(x => x.UpdateEmailVerification(It.IsAny<EmailVerificationDto>()))
            .ReturnsAsync(Result.Failure(UserErrors.ClientUpdateFailed));

        // Act
        var result = await authenticationService.VerifyEmail(token, login.Email);

        // Assert
        Assert.True(result.IsFailure);
        Assert.Equal(UserErrors.ClientEmailVerificationFailed.Message, result.Error.Message);
    }

    [Fact]
    public async Task DisconnectTest_ValidToken_ReturnsSuccess()
    {
        // Arrange
        var accessToken = "validtoken";
        var sessionId = "session";
        var userId = 123;
        tokenProviderMock.Setup(x => x.GetSessionId(accessToken)).Returns(sessionId);
        userQueryMock.Setup(x => x.GetUserId(sessionId)).ReturnsAsync(Result<int>.Success(userId));
        tokenProviderMock.Setup(x => x.InvalidateTokens(userId)).ReturnsAsync(Result.Success());

        // Act
        var result = await authenticationService.Disconnect(userId);

        // Assert
        Assert.True(result.IsSuccess);
    }

    [Fact]
    public async Task DisconnectTest_InvaliduserId_ReturnsFailure()
    {
        // Arrange
        var userId = 123;
        tokenProviderMock.Setup(x => x.InvalidateTokens(It.IsAny<int>()))
            .ReturnsAsync(Result<int>.Failure(UserErrors.ClientNotFound));

        // Act
        var result = await authenticationService.Disconnect(userId);

        // Assert
        Assert.True(result.IsFailure);
        Assert.Equal(UserErrors.ClientDisconnectFailed.Message, result.Error.Message);
    }
}
