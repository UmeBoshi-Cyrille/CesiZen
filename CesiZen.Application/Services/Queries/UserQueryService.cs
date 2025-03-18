using CesiZen.Domain.Datamodel;
using CesiZen.Domain.Interface;

namespace CesiZen.Application.Services;

public class UserQueryService : IUserQueryService
{
    private readonly IUserQuery queries;

    public UserQueryService(IUserQuery queries)
    {
        this.queries = queries;
    }

    public Task<IResult<IEnumerable<User>>> GetAllAsync()
    {
        throw new NotImplementedException();
    }

    public Task<IResult<IEnumerable<User>>> GetAllPaginatedAsync(int pageNumber, int pageSize)
    {
        throw new NotImplementedException();
    }

    public Task<IResult<User>> GetByIdAsync(int id)
    {
        throw new NotImplementedException();
    }

    public Task<IResult<User>> GetByUsername(string username)
    {
        throw new NotImplementedException();
    }
}
