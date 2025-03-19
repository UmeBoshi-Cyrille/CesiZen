using CesiZen.Domain.BusinessResult;
using CesiZen.Domain.Interfaces;
using CesiZen.Infrastructure.DatabaseContext;
using Microsoft.EntityFrameworkCore;

namespace CesiZen.Infrastructure.Repositories;

public class RefreshTokenQuery : AbstractRepository, IRefreshTokenQuery
{
    public RefreshTokenQuery(
        MongoDbContext context) :
        base(context)
    {
    }

    public async Task<IResult<RefreshToken>> GetById(string userId)
    {
        var result = await context.RefreshTokens
                .AsNoTracking()
                .FirstOrDefaultAsync(context => context.UserId == userId);

        if (result == null)
        {
            return Result<RefreshToken>.Failure(RefreshTokenErrors.LogNotFound(userId));
        }

        return Result<RefreshToken>.Success(result);
    }

    public async Task<IResult<string>> GetId(string userId)
    {
        var result = await context.RefreshTokens
                .AsNoTracking()
                .Where(context => context.UserId == userId)
                .Select(p => p.Id)
                .FirstOrDefaultAsync();

        if (string.IsNullOrEmpty(result))
        {
            return Result<string>.Failure(RefreshTokenErrors.LogNotFound(userId));
        }

        return Result<string>.Success(result);
    }
}

