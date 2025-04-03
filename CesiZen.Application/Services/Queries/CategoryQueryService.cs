using CesiZen.Domain.BusinessResult;
using CesiZen.Domain.DataTransfertObject;
using CesiZen.Domain.Interfaces;
using Serilog;

namespace CesiZen.Application.Services;

public class CategoryQueryService : AService, ICategoryQueryService
{
    private readonly ICategoryQuery query;

    public CategoryQueryService(ILogger logger, ICategoryQuery query) : base(logger)
    {
        this.query = query;
    }

    public async Task<IResult<PagedResultDto<CategoryResponseDto>>> GetAllAsync(int pageNumber, int pageSize)
    {
        var result = await query.GetAllAsync(pageNumber, pageSize);

        if (result.IsFailure)
        {
            logger.Error(result.Error.Message);
            return Result<PagedResultDto<CategoryResponseDto>>.Failure(CategoryErrors.ClientMultipleNotFound);
        }

        return Result<PagedResultDto<CategoryResponseDto>>.Success(result.Value);
    }

    public async Task<IResult<CategoryResponseDto>> GetByIdAsync(int id)
    {
        var result = await query.GetByIdAsync(id);

        if (result.IsFailure)
        {
            logger.Error(result.Error.Message);
            return Result<CategoryResponseDto>.Failure(CategoryErrors.ClientNotFound);
        }

        return Result<CategoryResponseDto>.Success(result.Value);
    }
}
