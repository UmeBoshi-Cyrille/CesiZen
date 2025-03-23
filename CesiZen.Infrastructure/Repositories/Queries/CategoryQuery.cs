using CesiZen.Domain.BusinessResult;
using CesiZen.Domain.Datamodel;
using CesiZen.Domain.DataTransfertObject;
using CesiZen.Domain.Interfaces;
using CesiZen.Infrastructure.DatabaseContext;
using Microsoft.EntityFrameworkCore;

namespace CesiZen.Infrastructure.Repositories;

public class CategoryQuery : AbstractRepository, ICategoryQuery
{
    public CategoryQuery(CesizenDbContext context) : base(context)
    {
    }

    public async Task<IResult<PagedResultDto<Category>>> GetAllAsync(int pageNumber, int pageSize)
    {
        try
        {
            var data = await context.Set<Category>()
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            var result = new PagedResultDto<Category>
            {
                Data = data,
                TotalCount = data.Count,
                PageNumber = pageNumber,
                PageSize = pageSize
            };

            return Result<PagedResultDto<Category>>.Success(result);
        }
        catch (Exception ex)
        {
            return Result<PagedResultDto<Category>>.Failure(CategoryErrors.LogMultipleNotFound, ex.Message);
        }
    }

    public async Task<IResult<Category>> GetByIdAsync(int id)
    {
        try
        {
            var result = await context.Categories.FindAsync(id);

            return Result<Category>.Success(result!);
        }
        catch (Exception ex)
        {
            return Result<Category>.Failure(CategoryErrors.LogNotFound(nameof(id)), nameof(id), ex.Message);
        }
    }
}

