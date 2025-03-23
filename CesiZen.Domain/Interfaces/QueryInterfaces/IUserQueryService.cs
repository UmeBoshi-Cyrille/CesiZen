using CesiZen.Domain.DataTransfertObject;

namespace CesiZen.Domain.Interfaces;

public interface IUserQueryService : IQueryServiceInterface<UserRequestDto>
{
    Task<IResult<PagedResultDto<UserRequestDto>>> SearchUsers(PageParametersDto parameters, string searchTerm);
    Task<IResult<UserRequestDto>> GetByUsername(string username);
}
