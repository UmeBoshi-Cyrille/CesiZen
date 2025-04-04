using CesiZen.Domain.BusinessResult;
using CesiZen.Domain.Datamodel;
using CesiZen.Domain.Interfaces;
using CesiZen.Infrastructure.DatabaseContext;
using EntityFramework.Exceptions.Common;
using Microsoft.EntityFrameworkCore;

namespace CesiZen.Infrastructure.Repositories;

public class SessionCommand : AbstractRepository, ISessionCommand
{
    public SessionCommand(CesizenDbContext context) : base(context)
    {
    }

    public async Task<IResult> UpSert(Session entity)
    {
        try
        {
            var hasSession = context.Sessions
                .Any(r => r.UserId == entity.UserId);

            if (hasSession)
            {
                await context.Sessions.ExecuteUpdateAsync(x => x
                .SetProperty(x => x.SessionId, entity.SessionId));
            }
            else
            {
                context.Entry(entity).State = EntityState.Added;

                await context.SaveChangesAsync();
            }

            return Result.Success();
        }
        catch (UniqueConstraintException ex)
        {
            return Result.Failure(SessionErrors.LogNotUnique(entity.SessionId), entity.SessionId, ex.Message);
        }
        catch (DbUpdateException ex)
        {
            return Result.Failure(SessionErrors.LogUpdateFailed(entity.SessionId), entity.SessionId, ex.Message);
        }
    }

    public async Task<IResult> Delete(int id)
    {
        try
        {
            var result = await context.Sessions
                 .Where(s => s.Id == id)
                 .ExecuteDeleteAsync();

            if (result <= 0)
            {
                return Result.Failure(SessionErrors.LogDeletionFailed(nameof(id)));
            }

            return Result.Success();
        }
        catch (DbUpdateException ex)
        {
            return Result.Failure(SessionErrors.LogDeletionFailed(nameof(id)), nameof(id), ex.Message);
        }
    }
}
