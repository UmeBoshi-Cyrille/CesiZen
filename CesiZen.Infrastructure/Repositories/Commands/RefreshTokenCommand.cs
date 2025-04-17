using CesiZen.Domain.BusinessResult;
using CesiZen.Domain.Datamodel;
using CesiZen.Domain.Interfaces;
using CesiZen.Infrastructure.DatabaseContext;
using EntityFramework.Exceptions.Common;
using Microsoft.EntityFrameworkCore;

namespace CesiZen.Infrastructure.Repositories;

public class TokenCommand : AbstractRepository, IRefreshTokenCommand
{
    public TokenCommand(CesizenDbContext context) : base(context)
    {
    }

    public async Task<IResult> UpSert(RefreshToken entity)
    {
        try
        {
            var hasToken = context.RefreshTokens.Any(r => r.UserId == entity.UserId);

            if (hasToken)
            {
                await context.RefreshTokens.ExecuteUpdateAsync(x => x
                .SetProperty(x => x.Token, entity.Token)
                .SetProperty(x => x.ExpirationTime, entity.ExpirationTime));
            }
            else
            {
                context.RefreshTokens.Add(entity);
                await context.SaveChangesAsync();
            }

            return Result.Success();
        }
        catch (UniqueConstraintException ex)
        {
            return Result.Failure(RefreshTokenErrors.LogNotUnique(entity.Token), entity.Token, ex.Message);
        }
        catch (DbUpdateException ex)
        {
            return Result.Failure(RefreshTokenErrors.LogUpdateFailed(entity.Token), entity.Token, ex.Message);
        }
    }

    public async Task<IResult> Delete(int id)
    {
        var result = await context.RefreshTokens
                .Where(x => x.Id == id)
                .ExecuteDeleteAsync();

        if (result <= 0)
        {
            return Result.Failure(RefreshTokenErrors.LogDeletionFailed(nameof(id)));
        }

        return Result.Success();
    }

    private void Insert(RefreshToken entity)
    {

    }
}
