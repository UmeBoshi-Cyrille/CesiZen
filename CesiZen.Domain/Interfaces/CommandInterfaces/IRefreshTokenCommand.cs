using CesiZen.Domain.Interfaces;

namespace CesiZen.Domain.Interfaces;

public interface IRefreshTokenCommand
{
    Task<IResult> UpSert(RefreshToken entity);

    Task<IResult> Delete(string id);
}
