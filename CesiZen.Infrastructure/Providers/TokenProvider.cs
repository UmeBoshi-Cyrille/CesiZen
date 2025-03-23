using CesiZen.Domain.BusinessResult;
using CesiZen.Domain.Datamodel;
using CesiZen.Domain.DataTransfertObject;
using CesiZen.Domain.Interfaces;
using Microsoft.IdentityModel.JsonWebTokens;
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
    private readonly IRefreshTokenCommand tokenCommand;
    private readonly IRefreshTokenQuery tokenQuery;
    private readonly ILogger logger;
    private readonly JwtSettings jwtSettings;

    public TokenProvider(
        ISessionQuery sessionQuery,
        ISessionCommand sessionCommand,
        IRefreshTokenCommand tokenCommand,
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
    }

    #region Public Methods
    public string GenerateAccessToken(TokenIdDto dto)
    {
        var builder = new TokenBuilderDto()
        {
            SessionId = dto.SessionId!,
            TokenId = dto.TokenId!,
            ExpirationTime = jwtSettings.ExpirationMinutes
        };
        var token = GenerateAccessToken(builder, jwtSettings);

        return token;
    }

    public IResult<TokenIdDto> GenerateRefreshToken(int userId)
    {
        var sessionId = GenerateSessionId();
        var tokenId = GenerateTokenId();
        var refreshToken = GenerateRefreshToken(sessionId, tokenId);

        var session = new Session(sessionId, userId);
        sessionCommand.UpSert(session);
        SaveRefreshToken(userId, refreshToken);

        var dto = new TokenIdDto()
        {
            SessionId = sessionId,
            TokenId = tokenId,
        };

        return Result<TokenIdDto>.Success(dto);
    }

    public async Task<IResult<string>> RefreshAccessTokenAsync(string accessToken)
    {
        if (CheckAccessTokenExpirationTime(accessToken))
        {
            return Result<string>.Success(accessToken, Info.Success("Token is still valid"));
        };

        var check = await CheckRefreshTokenValidity(accessToken);

        if (check)
        {
            var token = RefreshAccessToken(accessToken);

            return Result<string>.Success(token);
        }

        var sessionId = GetSessionId(accessToken);
        var session = await sessionQuery.GetBySessionId(sessionId!);

        await InvalidateTokens(session.Value.UserId);

        return Result<string>.Failure(Error.AuthenticationFailed("Token has expired"));
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

    public string? GetSessionId(string token)
    {
        var principal = GetAccessTokenPrincipal(token!);
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
        var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey!));

        var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(
            [
                new Claim("token_id", builder.TokenId),
                new Claim("session_id", builder.SessionId),
            ]),
            Expires = DateTime.UtcNow.AddMinutes(jwtSettings.ExpirationMinutes),
            SigningCredentials = credentials,
            Issuer = jwtSettings.Issuer,
            Audience = jwtSettings.Audience,
        };

        var handler = new JsonWebTokenHandler();

        string token = handler.CreateToken(tokenDescriptor);

        return token;
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

    private string RefreshAccessToken(string accessToken)
    {
        TokenIdDto dto = new();

        var principal = GetAccessTokenPrincipal(accessToken!);
        dto.TokenId = principal?.FindFirstValue("token_id")!;
        dto.SessionId = principal?.FindFirstValue("session_id")!;

        return GenerateAccessToken(dto);
    }

    private ClaimsPrincipal GetAccessTokenPrincipal(string token)
    {
        var validation = new TokenValidationParameters
        {
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings.SecretKey!)),
            ValidateIssuer = true,
            ValidateAudience = true,
            ClockSkew = TimeSpan.Zero,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = jwtSettings.Issuer,
            ValidAudience = jwtSettings.Audience,
        };

        return new JwtSecurityTokenHandler().ValidateToken(token, validation, out _);
    }

    private bool CheckAccessTokenExpirationTime(string token)
    {
        var expirationTime = GetAccessTokenExpirationTime(token);
        var remainingTime = expirationTime - DateTime.UtcNow;

        return remainingTime < TimeSpan.FromMinutes(5) ? true : false;
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
        var convertedTokenId = Convert.FromBase64String(tokenId);

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

    private async Task<bool> CheckRefreshTokenValidity(string providedAccessToken)
    {
        var sessionId = GetSessionId(providedAccessToken);
        var session = await sessionQuery.GetBySessionId(sessionId!);
        var currentToken = await tokenQuery.GetById(session.Value.UserId);
        var providedToken = GetProvidedRefreshToken(providedAccessToken);

        if (IsRefreshTokenValid(currentToken.Value.Token, providedToken) &&
            !IsTokenTimeOut(currentToken.Value.ExpirationTime))
        {
            return true;
        }

        return false;
    }

    private string GetProvidedRefreshToken(string token)
    {
        var secret = jwtSettings.RefreshSecret;

        var principal = GetAccessTokenPrincipal(token!);
        var rawTokenId = principal?.FindFirstValue("token_id");
        var sessionId = principal?.FindFirstValue("session_id");

        var tokenId = Convert.FromBase64String(rawTokenId!);

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
