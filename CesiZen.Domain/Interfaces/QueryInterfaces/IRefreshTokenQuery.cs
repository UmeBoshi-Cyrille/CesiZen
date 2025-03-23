using CesiZen.Domain.Datamodel;

namespace CesiZen.Domain.Interfaces;

public interface IRefreshTokenQuery
{
    Task<IResult<RefreshToken>> GetById(int userId);

    Task<IResult<int>> GetId(int userId);
}
