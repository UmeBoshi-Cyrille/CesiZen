using CesiZen.Domain.Datamodel;

namespace CesiZen.Domain.Interface;

public interface IUserQuery : IQueryInterface<User>
{
    Task<IResult<User>> GetByUsername(string username);

    Task<IResult<IEnumerable<User>>> GetAllPaginatedAsync(int pageNumber, int pageSize);

    Task<IResult<string>> GetUserId(string sessionId);
}
