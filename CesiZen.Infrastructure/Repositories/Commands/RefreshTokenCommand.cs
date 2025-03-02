using CesiZen.Domain.BusinessResult;
using CesiZen.Domain.Interface;
using CesiZen.Domain.Interfaces;
using CesiZen.Infrastructure.DatabaseContext;
using CesiZen.Infrastructure.Repository;
using EntityFramework.Exceptions.Common;
using Microsoft.EntityFrameworkCore;

namespace CesiZen.Infrastructure.Repositories;

public class TokenCommand : AbstractRepository, IRefreshTokenCommand
{
    public TokenCommand(MongoDbContext context) : base(context)
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
        }
        catch (UniqueConstraintException ex)
        {
            return Result.Failure(
                Error.NotUnique(string.Format(
                    Message.GetResource("ErrorMessages", "LOG_CHECK_UNICITY_CONSTRAINT"), "RefreshToken", entity.Id)));
        }
        catch (DbUpdateException ex)
        {
            return Result.Failure(
               Error.OperationFailed(string.Format(
                   Message.GetResource("ErrorMessages", "LOG_UPDATE_OPERATIONFAILED"), "RefreshToken", entity.Id)));
        }

        return Result.Success();
    }

    public async Task<IResult> Delete(string id)
    {
        var result = await context.RefreshTokens
                .Where(x => x.Id == id)
                .ExecuteDeleteAsync();

        if (result <= 0)
        {
            return Result.Failure(
                Error.OperationFailed(string.Format(
                    Message.GetResource("ErrorMessages", "LOG_DELETE_OPERATIONFAILED"), "RefreshToken", id)));
        }

        return Result.Success();
    }

    private void Insert(RefreshToken entity)
    {
        context.RefreshTokens.Add(entity);
    }
}
