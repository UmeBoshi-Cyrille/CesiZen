using CesiZen.Domain.DataTransfertObject;

namespace CesiZen.Domain.Interface;

public interface IUserQueryService : IQueryServiceInterface<UserRequestDto>
{
    Task<IResult<PagedResult<UserRequestDto>>> SearchUsers(PageParameters parameters, string searchTerm);
    Task<IResult<UserRequestDto>> GetByUsername(string username);
}
