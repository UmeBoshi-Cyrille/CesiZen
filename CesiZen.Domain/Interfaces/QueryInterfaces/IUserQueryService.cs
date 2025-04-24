using CesiZen.Domain.DataTransfertObject;

namespace CesiZen.Domain.Interfaces;

public interface IUserQueryService
{
    Task<IResult<PagedResultDto<UserMinimumDto>>> SearchUsers(PageParametersDto parameters, string searchTerm);
    Task<IResult<UserDto>> GetByUsername(string username);
    Task<IResult<UserDto>> GetByIdAsync(int id);
    Task<IResult<PagedResultDto<UserMinimumDto>>> GetAllAsync(int pageNumber, int pageSize);
    Task<IResult<UserProfileDto>> GetProfile(int id);
}
