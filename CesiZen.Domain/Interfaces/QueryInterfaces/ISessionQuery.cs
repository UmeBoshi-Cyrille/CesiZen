namespace CesiZen.Domain.Interfaces;

public interface ISessionQuery
{
    Task<IResult<int>> GetId(int userId);
}

