using CesiZen.Domain.Datamodel;
using CesiZen.Domain.DataTransfertObject;

namespace CesiZen.Domain.Interfaces;

public interface IUserQuery : IQueryInterface<User>
{
    Task<IResult<PagedResult<User>>> SearchUsers(PageParameters parameters, string searchTerm);

    Task<IResult<User>> GetByUsername(string username);

    Task<IResult<string>> GetUserId(string sessionId);
}
