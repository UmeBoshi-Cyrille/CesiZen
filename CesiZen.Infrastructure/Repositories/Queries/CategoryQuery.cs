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

    public async Task<IResult<PagedResultDto<CategoryDto>>> GetAllAsync(int pageNumber, int pageSize)
    {
        try
        {
            var data = await context.Set<Category>()
                .OrderBy(x => x.Name)
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .Select(x => x.Map())
                .ToListAsync();

            var result = new PagedResultDto<CategoryDto>
            {
                Data = data,
                TotalCount = data.Count,
                PageNumber = pageNumber,
                PageSize = pageSize
            };

            return Result<PagedResultDto<CategoryDto>>.Success(result);
        }
        catch (Exception ex)
        {
            return Result<PagedResultDto<CategoryDto>>.Failure(CategoryErrors.LogMultipleNotFound, ex.Message);
        }
    }

    public async Task<IResult<CategoryDto>> GetByIdAsync(int id)
    {
        try
        {
            var result = await context.Categories
                .Where(x => x.Id == id)
                .Select(x => x.Map())
                .FirstOrDefaultAsync();

            return Result<CategoryDto>.Success(result!);
        }
        catch (Exception ex)
        {
            return Result<CategoryDto>.Failure(CategoryErrors.LogNotFound(nameof(id)), nameof(id), ex.Message);
        }
    }
}

