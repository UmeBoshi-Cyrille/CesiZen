using CesiZen.Domain.Datamodel;

namespace CesiZen.Domain.Interfaces;

public interface ISessionCommand
{
    Task<IResult> UpSert(Session entity);

    Task<IResult> Delete(string id);
}
