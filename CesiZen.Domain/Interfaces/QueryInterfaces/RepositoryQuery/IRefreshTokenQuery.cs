using CesiZen.Domain.Interface;

namespace CesiZen.Domain.Interfaces;

public interface IRefreshTokenQuery
{
    Task<IResult<RefreshToken>> GetById(string loginId);

    Task<IResult<int>> GetId(string userId);
}
