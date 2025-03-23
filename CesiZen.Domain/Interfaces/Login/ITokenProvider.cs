using CesiZen.Domain.Interfaces;

namespace CesiZen.Domain.Interfaces;

public interface ITokenProvider
{
    string GenerateAccessToken(int userId);

    Task<IResult<string>> RefreshAccessTokenAsync(int userId, string accessToken);

    Task<IResult> InvalidateTokens(int userId);

    bool CheckAccessTokenExpirationTime(string token);

    string GetTokenSessionId(string token);

    string GenerateVerificationToken();
}
