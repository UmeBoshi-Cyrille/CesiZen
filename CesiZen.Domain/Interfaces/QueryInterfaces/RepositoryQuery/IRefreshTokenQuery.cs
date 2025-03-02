using CesiZen.Domain.Interface;

namespace CesiZen.Domain.Interfaces;

public interface IRefreshTokenQuery
{
    Task<IResult<RefreshToken>> GetById(string loginId);

    Task<IResult<string>> GetId(string userId);
}
