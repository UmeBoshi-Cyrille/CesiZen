using CesiZen.Domain.BusinessResult;
using CesiZen.Domain.Interface;
using CesiZen.Domain.Interfaces;
using CesiZen.Infrastructure.DatabaseContext;
using Microsoft.EntityFrameworkCore;

namespace CesiZen.Infrastructure.Repository;

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
            return Result<RefreshToken>.Failure(
                Error.NotFound(string.Format(
                    Message.GetResource("ErrorMessages", "LOG_GETONE_NOTFOUND"), "RefreshToken", userId)));
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
            return Result<string>.Failure(
                Error.NotFound(string.Format(
                    Message.GetResource("ErrorMessages", "LOG_GETONE_NOTFOUND"), "RefreshToken", userId)));
        }

        return Result<string>.Success(result);
    }
}

