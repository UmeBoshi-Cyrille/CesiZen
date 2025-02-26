using CesiZen.Domain.Interface;

namespace CesiZen.Domain.Interfaces;

public interface IRefreshTokenQuery
{
    Task<IResult<RefreshToken>> GetById(int loginId);

    Task<IResult<int>> GetId(int userId);
}
