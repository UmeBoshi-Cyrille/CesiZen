using CesiZen.Domain.BusinessResult;
using CesiZen.Domain.Datamodel;
using CesiZen.Domain.DataTransfertObject;
using CesiZen.Domain.Interfaces;
using CesiZen.Domain.Mapper;
using CesiZen.Infrastructure.DatabaseContext;
using Microsoft.EntityFrameworkCore;

namespace CesiZen.Infrastructure.Repositories;

public class CategoryQuery : AbstractRepository, ICategoryQuery
{
    public CategoryQuery(CesizenDbContext context) : base(context)
    {
    }

    public async Task<IResult<PagedResultDto<CategoryResponseDto>>> GetAllAsync(int pageNumber, int pageSize)
    {
        try
        {
            var data = await context.Set<Category>()
                .OrderBy(x => x.Name)
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .Select(x => x.MapResponseDto())
                .ToListAsync();

            var result = new PagedResultDto<CategoryResponseDto>
            {
                Data = data,
                TotalCount = data.Count,
                PageNumber = pageNumber,
                PageSize = pageSize
            };

            return Result<PagedResultDto<CategoryResponseDto>>.Success(result);
        }
        catch (Exception ex)
        {
            return Result<PagedResultDto<CategoryResponseDto>>.Failure(CategoryErrors.LogMultipleNotFound, ex.Message);
        }
    }

    public async Task<IResult<CategoryResponseDto>> GetByIdAsync(int id)
    {
        try
        {
            var result = await context.Categories
                .Where(x => x.Id == id)
                .Select(x => x.MapResponseDto())
                .FirstOrDefaultAsync();

            return Result<CategoryResponseDto>.Success(result!);
        }
        catch (Exception ex)
        {
            return Result<CategoryResponseDto>.Failure(CategoryErrors.LogNotFound(nameof(id)), nameof(id), ex.Message);
        }
    }
}

