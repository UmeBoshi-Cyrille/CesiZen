using CesiZen.Domain.Datamodel;

namespace CesiZen.Domain.Interface;

public interface IUserQueryService : IQueryServiceInterface<User>
{
    Task<IResult<User>> GetByUsername(string username);

    Task<IResult<IEnumerable<User>>> GetAllPaginatedAsync(int pageNumber, int pageSize);
}
