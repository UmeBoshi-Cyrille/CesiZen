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

    public async Task<IResult<PagedResultDto<CategoryDto>>> GetAllAsync(int pageNumber, int pageSize)
    {
        var result = await query.GetAllAsync(pageNumber, pageSize);

        if (result.IsFailure)
        {
            logger.Error(result.Error.Message);
            return Result<PagedResultDto<CategoryDto>>.Failure(CategoryErrors.ClientMultipleNotFound);
        }

        return Result<PagedResultDto<CategoryDto>>.Success(result.Value);
    }

    public async Task<IResult<CategoryDto>> GetByIdAsync(int id)
    {
        var result = await query.GetByIdAsync(id);

        if (result.IsFailure)
        {
            logger.Error(result.Error.Message);
            return Result<CategoryDto>.Failure(CategoryErrors.ClientNotFound);
        }

        return Result<CategoryDto>.Success(result.Value);
    }
}
