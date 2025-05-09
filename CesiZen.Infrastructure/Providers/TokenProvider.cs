using CesiZen.Domain.BusinessResult;
using CesiZen.Domain.Datamodel;
using CesiZen.Domain.DataTransfertObject;
using CesiZen.Domain.Interfaces;
using CesiZen.Domain.Mapper;
using Microsoft.IdentityModel.Tokens;
using Serilog;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace CesiZen.Infrastructure.Providers;

public class TokenProvider : ITokenProvider
{
    private readonly ISessionQuery sessionQuery;
    private readonly ISessionCommand sessionCommand;
    private readonly IUserQuery userQuery;
    private readonly IRefreshTokenCommand tokenCommand;
    private readonly IRefreshTokenQuery tokenQuery;
    private readonly ILogger logger;
    private readonly JwtSettings jwtSettings;

    public TokenProvider(
        ISessionQuery sessionQuery,
        ISessionCommand sessionCommand,
        IRefreshTokenCommand tokenCommand,
        IUserQuery userQuery,
        IRefreshTokenQuery tokenQuery,
        ILogger logger,
        JwtSettings jwtSettings)
    {
        this.sessionQuery = sessionQuery;
        this.sessionCommand = sessionCommand;
        this.tokenCommand = tokenCommand;
        this.tokenQuery = tokenQuery;
        this.logger = logger;
        this.jwtSettings = jwtSettings;
        this.userQuery = userQuery;
    }

    #region Public Methods
    public string GenerateAccessToken(TokenBuilderDto dto)
    {
        var token = GenerateAccessToken(dto, jwtSettings);

        return token;
    }

    public async Task<IResult<TokenBuilderDto>> GenerateRefreshToken(int userId)
    {
        var sessionId = GenerateSessionId();
        var tokenId = GenerateTokenId();
        var refreshToken = GenerateRefreshToken(sessionId, tokenId);

        var session = new Session(sessionId, userId);
        await sessionCommand.UpSert(session);
        SaveRefreshToken(userId, refreshToken);

        var dto = new TokenBuilderDto()
        {
            SessionId = sessionId,
            TokenId = tokenId,
            ExpirationTime = jwtSettings.ExpirationMinutes,
        };

        return Result<TokenBuilderDto>.Success(dto);
    }

    public async Task<IResult<AuthenticateResponseDto>> RefreshAccessTokenAsync(string accessToken, ClaimsPrincipal principal)
    {
        var result = new AuthenticateResponseDto();
        var userId = principal?.FindFirst(ClaimTypes.NameIdentifier)!.Value;
        var user = await userQuery.GetByIdAsync(int.Parse(userId!));

        if (CheckAccessTokenExpirationTime(accessToken))
        {
            result.Token = accessToken;
            result.TokenExpirationTime = jwtSettings.ExpirationMinutes;
            result.IsLoggedIn = true;
            result.User = user.Value.Map();
            return Result<AuthenticateResponseDto>.Success(result, Info.Success("Token is still valid"));
        };

        var check = await CheckRefreshTokenValidity(principal);

        if (check)
        {
            var token = RefreshAccessToken(principal);

            result.Token = token;
            result.TokenExpirationTime = jwtSettings.ExpirationMinutes;
            result.IsLoggedIn = true;
            result.User = user.Value.Map();
            return Result<AuthenticateResponseDto>.Success(result, RefreshTokenInfos.TokenRenewed);
        }

        var sessionId = GetSessionId(principal);
        var session = await sessionQuery.GetBySessionId(sessionId!);

        await InvalidateTokens(session.Value.UserId);

        return Result<AuthenticateResponseDto>.Failure(Error.AuthenticationFailed("Token has expired"));
    }

    public async Task<IResult> InvalidateTokens(int userId)
    {
        var session = await sessionQuery.GetId(userId);
        var tokenId = await tokenQuery.GetId(userId);

        var sessionResult = await sessionCommand.Delete(session.Value);
        var tokenResult = await tokenCommand.Delete(tokenId.Value);

        if (sessionResult.IsFailure)
        {
            logger.Error(sessionResult.Error.Message);
            return Result.Failure(UserErrors.ClientDisconnectFailed);
        }
        else if (tokenResult.IsFailure)
        {
            logger.Error(tokenResult.Error.Message);
            return Result.Failure(UserErrors.ClientDisconnectFailed);
        }

        return Result.Success();
    }

    public string GenerateVerificationToken()
    {
        return Convert.ToBase64String(Guid.NewGuid().ToByteArray());
    }

    public string? GetSessionId(ClaimsPrincipal principal)
    {
        var sessionId = principal?.FindFirstValue("session_id");

        return sessionId;
    }
    #endregion

    #region Private AccessToken methods
    private string GenerateAccessToken(
        TokenBuilderDto builder,
        JwtSettings jwtSettings)
    {
        string? secretKey = jwtSettings.SecretKey;
        var securityKey = new SymmetricSecurityKey(Convert.FromBase64String(secretKey!));

        var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);
        var issuer = jwtSettings.Issuer;
        var audience = jwtSettings.Audience;
        var expirationMinutes = jwtSettings.ExpirationMinutes;

        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(
            [
                new Claim(ClaimTypes.NameIdentifier, builder.UserId.ToString()),
                new Claim(ClaimTypes.Role, builder.Role),
                new Claim("token_id", builder.TokenId),
                new Claim("session_id", builder.SessionId),
            ]),
            Expires = DateTime.UtcNow.AddMinutes(expirationMinutes),
            SigningCredentials = credentials,
            Issuer = issuer,
            Audience = audience,
        };

        var handler = new JwtSecurityTokenHandler();

        var token = handler.CreateToken(tokenDescriptor);
        var securedToken = handler.WriteToken(token);

        Console.WriteLine(securedToken);

        return securedToken;
    }

    private string GenerateTokenId()
    {
        var randomNumber = new byte[64];

        using (var numberGenerator = RandomNumberGenerator.Create())
        {
            numberGenerator.GetBytes(randomNumber);
        }

        string tokenId = Convert.ToBase64String(randomNumber);

        return tokenId;
    }

    private string GenerateSessionId()
    {
        return Guid.NewGuid().ToString();
    }

    private string RefreshAccessToken(ClaimsPrincipal principal)
    {
        TokenBuilderDto dto = new();

        dto.TokenId = principal?.FindFirstValue("token_id")!;
        dto.SessionId = principal?.FindFirstValue("session_id")!;

        return GenerateAccessToken(dto);
    }

    private bool CheckAccessTokenExpirationTime(string token)
    {
        var expirationTime = GetAccessTokenExpirationTime(token);
        var remainingTime = expirationTime - DateTime.UtcNow;

        return remainingTime < TimeSpan.FromMinutes(2) ? true : false;
    }

    private DateTime GetAccessTokenExpirationTime(string token)
    {
        var handler = new JwtSecurityTokenHandler();
        var jwtSecurityToken = handler.ReadJwtToken(token);
        return jwtSecurityToken.ValidTo;
    }
    #endregion

    #region private RefreshToken Methods
    private string GenerateRefreshToken(string sessionId, string tokenId)
    {
        var salt = jwtSettings.SecretKey;
        var convertedTokenId = Convert.FromBase64String(salt);

        var refreshToken = HashToken(sessionId, convertedTokenId);

        return refreshToken;
    }

    private string HashToken(string sessionId, byte[] salt)
    {
        const int keySize = 64;
        const int iterations = 350000;
        HashAlgorithmName hashAlgorithmName = HashAlgorithmName.SHA512;

        var secret = jwtSettings.RefreshSecret;

        var hash = Rfc2898DeriveBytes.Pbkdf2(
            Encoding.UTF8.GetBytes(sessionId + secret),
            salt,
            iterations,
            hashAlgorithmName,
            keySize
        );

        return Convert.ToHexString(hash);
    }

    private void SaveRefreshToken(int userId, string refreshToken)
    {
        var token = new RefreshToken()
        {
            Token = refreshToken,
            UserId = userId,
            ExpirationTime = DateTime.UtcNow.AddHours(2)
        };

        tokenCommand.UpSert(token);
    }

    private async Task<bool> CheckRefreshTokenValidity(ClaimsPrincipal principal)
    {
        var userId = principal?.FindFirst(ClaimTypes.NameIdentifier)!.Value;
        var currentToken = await tokenQuery.GetById(int.Parse(userId!));
        var providedToken = GetProvidedRefreshToken(principal!);

        if (IsRefreshTokenValid(currentToken.Value.Token, providedToken) &&
            !IsTokenTimeOut(currentToken.Value.ExpirationTime))
        {
            return true;
        }

        return false;
    }

    private string GetProvidedRefreshToken(ClaimsPrincipal principal)
    {
        var secret = jwtSettings.RefreshSecret;

        var rawTokenId = principal?.FindFirstValue("token_id");
        var sessionId = principal?.FindFirstValue("session_id");

        var salt = jwtSettings.SecretKey;
        var tokenId = Convert.FromBase64String(salt!);
        var providedRefreshToken = HashToken(sessionId!, tokenId);

        return string.IsNullOrEmpty(providedRefreshToken) ? "" : providedRefreshToken;
    }

    private bool IsRefreshTokenValid(string currentToken, string providedToken)
    {
        var currentHash = Convert.FromHexString(currentToken);
        var providedHash = Convert.FromHexString(providedToken);

        return CryptographicOperations.FixedTimeEquals(providedHash, currentHash);
    }

    private bool IsTokenTimeOut(DateTime? expirationTime)
    {
        var remainingTime = expirationTime - DateTime.UtcNow;

        return remainingTime <= TimeSpan.FromMinutes(5) ? true : false;
    }
    #endregion
}
