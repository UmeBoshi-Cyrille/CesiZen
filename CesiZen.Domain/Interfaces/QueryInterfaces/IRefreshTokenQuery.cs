using CesiZen.Domain.Datamodel;

namespace CesiZen.Domain.Interfaces;

public interface IRefreshTokenQuery
{
    Task<IResult<RefreshToken>> GetById(string userId);

    Task<IResult<string>> GetId(string userId);
}
