using CesiZen.Domain.BusinessResult;
using CesiZen.Domain.DataTransfertObject;
using CesiZen.Domain.Interfaces;
using CesiZen.Domain.Mapper;
using Serilog;

namespace CesiZen.Application.Services;

public class CategoryQueryService : AService, ICategoryQueryService
{
    private readonly ICategoryQuery query;

    public CategoryQueryService(ILogger logger, ICategoryQuery query) : base(logger)
    {
        this.query = query;
    }

    public async Task<IResult<PagedResultDto<CategoryRequestDto>>> GetAllAsync(int pageNumber, int pageSize)
    {
        var result = await query.GetAllAsync(pageNumber, pageSize);

        if (result.IsFailure)
        {
            logger.Error(result.Error.Message);
            return Result<PagedResultDto<CategoryRequestDto>>.Failure(CategoryErrors.ClientMultipleNotFound);
        }

        var dto = result.Value.Map();

        return Result<PagedResultDto<CategoryRequestDto>>.Success(dto);
    }

    public async Task<IResult<CategoryRequestDto>> GetByIdAsync(int id)
    {
        var result = await query.GetByIdAsync(id);

        if (result.IsFailure)
        {
            logger.Error(result.Error.Message);
            return Result<CategoryRequestDto>.Failure(CategoryErrors.ClientNotFound);
        }

        var dto = result.Value.MapDto();

        return Result<CategoryRequestDto>.Success(dto);
    }
}
