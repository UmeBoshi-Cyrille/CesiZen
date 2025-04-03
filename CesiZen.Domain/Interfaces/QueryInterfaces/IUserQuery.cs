using CesiZen.Domain.DataTransfertObject;

namespace CesiZen.Domain.Interfaces;

public interface IUserQuery
{
    Task<IResult<UserDto>> GetByIdAsync(int id);

    Task<IResult<PagedResultDto<UserMinimumDto>>> GetAllAsync(int pageNumber, int pageSize);

    Task<IResult<PagedResultDto<UserMinimumDto>>> SearchUsers(PageParametersDto parameters, string searchTerm);

    Task<IResult<UserDto>> GetByUsername(string username);

    Task<IResult<AuthenticationUserDto>> GetByIdentifier(string identifier, bool isEmail = false);

    Task<IResult<int>> GetUserId(string sessionId);
}
