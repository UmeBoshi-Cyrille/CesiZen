using CesiZen.Domain.Datamodel;
using CesiZen.Domain.Interface;

namespace CesiZen.Domain.Interfaces;

public interface ISessionCommand
{
    Task<IResult> UpSert(Session entity);

    Task<IResult> Delete(string id);
}
