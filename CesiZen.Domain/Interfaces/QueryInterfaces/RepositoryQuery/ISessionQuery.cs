using CesiZen.Domain.Interface;

namespace CesiZen.Domain.Interfaces;

public interface ISessionQuery
{
    Task<IResult<int>> GetId(int id);
}

