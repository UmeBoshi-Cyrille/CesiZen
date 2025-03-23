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
            var token = await context.RefreshTokens.FirstOrDefaultAsync(r => r.UserId == entity.UserId);

            if (token is not null)
            {
                token.Token = entity.Token;
                token.ExpirationTime = entity.ExpirationTime;
            }
            else
            {
                Insert(entity);
            }

            await context.SaveChangesAsync();

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

    public async Task<IResult> Delete(int Id)
    {
        var result = await context.RefreshTokens
                .Where(x => x.Id == id)
                .ExecuteDeleteAsync();

        if (result <= 0)
        {
            return Result.Failure(RefreshTokenErrors.LogDeletionFailed(id));
        }

        return Result.Success();
    }

    private void Insert(RefreshToken entity)
    {
        context.RefreshTokens.Add(entity);
    }
}
