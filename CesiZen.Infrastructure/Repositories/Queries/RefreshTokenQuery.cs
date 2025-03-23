using CesiZen.Domain.BusinessResult;
using CesiZen.Domain.Datamodel;
using CesiZen.Domain.Interfaces;
using CesiZen.Infrastructure.DatabaseContext;
using Microsoft.EntityFrameworkCore;

namespace CesiZen.Infrastructure.Repositories;

public class RefreshTokenQuery : AbstractRepository, IRefreshTokenQuery
{
    public RefreshTokenQuery(
        CesizenDbContext context) :
        base(context)
    {
    }

    public async Task<IResult<RefreshToken>> GetById(int userId)
    {
        var result = await context.RefreshTokens
                .AsNoTracking()
                .FirstOrDefaultAsync(context => context.UserId == userId);

        if (result == null)
        {
            return Result<RefreshToken>.Failure(RefreshTokenErrors.LogNotFound((nameof(userId))));
        }

        return Result<RefreshToken>.Success(result);
    }

    public async Task<IResult<int>> GetId(int userId)
    {
        var result = await context.RefreshTokens
                .AsNoTracking()
                .Where(context => context.UserId == userId)
                .Select(p => p.Id)
                .FirstOrDefaultAsync();

        if (result != 0)
        {
            return Result<int>.Success(result);
        }

        return Result<int>.Failure(RefreshTokenErrors.LogNotFound(nameof(userId)));
    }
}

