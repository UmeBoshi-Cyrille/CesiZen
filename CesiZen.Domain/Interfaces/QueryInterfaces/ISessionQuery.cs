using CesiZen.Domain.Datamodel;

namespace CesiZen.Domain.Interfaces;

public interface ISessionQuery
{
    Task<IResult<int>> GetId(int userId);

    Task<IResult<Session>> GetBySessionId(string sessionId);
}

