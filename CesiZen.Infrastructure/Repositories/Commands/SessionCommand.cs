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
            var session = await context.Sessions
                .FirstOrDefaultAsync(r => r.UserId == entity.UserId);

            if (session is not null)
            {
                session.SessionId = entity.SessionId;
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
            return Result.Failure(SessionErrors.LogNotUnique(entity.SessionId), entity.SessionId, ex.Message);
        }
        catch (DbUpdateException ex)
        {
            return Result.Failure(SessionErrors.LogUpdateFailed(entity.SessionId), entity.SessionId, ex.Message);
        }
    }

    public async Task<IResult> Delete(string sessionId)
    {
        try
        {
            var result = await context.Sessions
                 .Where(s => s.SessionId == sessionId)
                 .ExecuteDeleteAsync();

            if (result <= 0)
            {
                return Result.Failure(SessionErrors.LogDeletionFailed(sessionId));
            }

            return Result.Success();
        }
        catch (DbUpdateException ex)
        {
            return Result.Failure(SessionErrors.LogDeletionFailed(sessionId), sessionId, ex.Message);
        }
    }

    private void Insert(Session entity)
    {
        context.Sessions.Add(entity);
    }

    private async Task Update(string sessionId, Session entity)
    {
        await context.Sessions
                .Where(p => p.SessionId == sessionId)
                .ExecuteUpdateAsync(p => p
                    .SetProperty(p => p.SessionId, entity.SessionId));
    }
}
