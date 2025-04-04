using CesiZen.Domain.DataTransfertObject;

namespace CesiZen.Domain.Interfaces;

public interface IUserQueryService : IQueryServiceInterface<UserMinimumDto>
{
    Task<IResult<PagedResultDto<UserMinimumDto>>> SearchUsers(PageParametersDto parameters, string searchTerm);
    Task<IResult<UserDto>> GetByUsername(string username);
}
