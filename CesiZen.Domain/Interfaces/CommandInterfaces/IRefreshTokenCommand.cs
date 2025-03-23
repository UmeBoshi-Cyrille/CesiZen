using CesiZen.Domain.Datamodel;

namespace CesiZen.Domain.Interfaces;

public interface IRefreshTokenCommand
{
    Task<IResult> UpSert(RefreshToken entity);

    Task<IResult> Delete(int Id);
}
