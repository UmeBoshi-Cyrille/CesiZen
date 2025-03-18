using CesiZen.Domain.BusinessResult;
using CesiZen.Domain.DataTransfertObject;
using CesiZen.Domain.Interface;
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

    public async Task<IResult<PagedResult<UserRequestDto>>> SearchUsers(PageParameters parameters, string searchTerm)
    {
        var result = await query.SearchUsers(parameters, searchTerm);

        if (result.IsFailure)
        {
            logger.Error(result.Error.Message);
            return Result<PagedResult<UserRequestDto>>.Failure(Error.NullValue(""));
        }

        var dtos = result.Value.Map();

        return Result<PagedResult<UserRequestDto>>.Success(dtos);
    }

    public async Task<IResult<PagedResult<UserRequestDto>>> GetAllAsync(int pageNumber, int pageSize)
    {
        var result = await query.GetAllAsync(pageNumber, pageSize);

        if (result.IsFailure)
        {
            logger.Error(result.Error.Message);
            return Result<PagedResult<UserRequestDto>>.Failure(Error.NullValue(""));
        }

        var dto = result.Value.Map();

        return Result<PagedResult<UserRequestDto>>.Success(dto);
    }

    public async Task<IResult<UserRequestDto>> GetByIdAsync(int id)
    {
        var result = await query.GetByIdAsync(id);

        if (result.IsFailure)
        {
            logger.Error(result.Error.Message);
            return Result<UserRequestDto>.Failure(Error.NullValue(""));
        }

        var dto = result.Value.Map();

        return Result<UserRequestDto>.Success(dto);
    }

    public async Task<IResult<UserRequestDto>> GetByUsername(string username)
    {
        var result = await query.GetByUsername(username);

        if (result.IsFailure)
        {
            logger.Error(result.Error.Message);
            return Result<UserRequestDto>.Failure(Error.NullValue(""));
        }

        var dto = result.Value.Map();

        return Result<UserRequestDto>.Success(dto);
    }
}
