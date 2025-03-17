using CesiZen.Domain.Interface;

namespace CesiZen.Domain.Interfaces;

public interface ISessionQuery
{
    Task<IResult<string>> GetId(string id);
}

