using CesiZen.Domain.BusinessResult;
using CesiZen.Domain.DataTransfertObject;
using CesiZen.Domain.Interfaces;
using CesiZen.Domain.Mapper;
using Serilog;

namespace CesiZen.Application.Services;

public class UserQueryService : AService, IUserQueryService
{
    private readonly IUserQuery query;

    public UserQueryService(IUserQuery query, ILogger logger) : base(logger)
    {
        this.query = query;
    }

    public async Task<IResult<PagedResultDto<UserMinimumDto>>> SearchUsers(PageParametersDto parameters, string searchTerm)
    {
        var result = await query.SearchUsers(parameters, searchTerm);

        if (result.IsFailure)
        {
            logger.Error(result.Error.Message);
            return Result<PagedResultDto<UserMinimumDto>>.Failure(UserErrors.ClientMultipleNotFound);
        }

        var dtos = result.Value.Map();

        return Result<PagedResultDto<UserMinimumDto>>.Success(dtos);
    }

    public async Task<IResult<PagedResultDto<UserMinimumDto>>> GetAllAsync(int pageNumber, int pageSize)
    {
        var result = await query.GetAllAsync(pageNumber, pageSize);

        if (result.IsFailure)
        {
            logger.Error(result.Error.Message);
            return Result<PagedResultDto<UserMinimumDto>>.Failure(UserErrors.ClientMultipleNotFound);
        }

        var dto = result.Value.Map();

        return Result<PagedResultDto<UserMinimumDto>>.Success(dto);
    }

    public async Task<IResult<UserMinimumDto>> GetByIdAsync(int id)
    {
        var result = await query.GetByIdAsync(id);

        if (result.IsFailure)
        {
            logger.Error(result.Error.Message);
            return Result<UserMinimumDto>.Failure(UserErrors.ClientNotFound);
        }

        var dto = result.Value.Map();

        return Result<UserMinimumDto>.Success(dto);
    }

    public async Task<IResult<UserMinimumDto>> GetByUsername(string username)
    {
        var result = await query.GetByUsername(username);

        if (result.IsFailure)
        {
            logger.Error(result.Error.Message);
            return Result<UserMinimumDto>.Failure(UserErrors.ClientNotFound);
        }

        var dto = result.Value.Map();

        return Result<UserMinimumDto>.Success(dto);
    }
}
