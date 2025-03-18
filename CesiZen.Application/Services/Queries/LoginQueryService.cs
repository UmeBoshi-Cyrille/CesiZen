using CesiZen.Domain.Datamodel;
using CesiZen.Domain.Interface;

namespace CesiZen.Application.Services;

public class LoginQueryService : ILoginQueryService
{
    private readonly ILoginQuery queries;

    public LoginQueryService(ILoginQuery queries)
    {
        this.queries = queries;
    }

    public Task<IResult<Login>> GetByEmail(string email)
    {
        throw new NotImplementedException();
    }

    public Task<IResult<Login>> GetByUserId(int userId)
    {
        throw new NotImplementedException();
    }
}
