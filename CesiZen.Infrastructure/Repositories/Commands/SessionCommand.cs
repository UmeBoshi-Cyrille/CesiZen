using CesiZen.Domain.BusinessResult;
using CesiZen.Domain.Datamodel;
using CesiZen.Domain.Interface;
using CesiZen.Domain.Interfaces;
using CesiZen.Infrastructure.DatabaseContext;
using CesiZen.Infrastructure.Repository;
using EntityFramework.Exceptions.Common;
using Microsoft.EntityFrameworkCore;

namespace CesiZen.Infrastructure.Repositories;

public class SessionCommand : AbstractRepository, ISessionCommand
{
    public SessionCommand(MongoDbContext context) : base(context)
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
        }
        catch (UniqueConstraintException ex)
        {
            return Result.Failure(
                Error.NotUnique(string.Format(
                    Message.GetResource("ErrorMessages", "LOG_CHECK_UNICITY_CONSTRAINT"), "Session", entity.SessionId)));
        }
        catch (DbUpdateException ex)
        {
            return Result.Failure(
               Error.OperationFailed(string.Format(
                   Message.GetResource("ErrorMessages", "LOG_UPDATE_OPERATIONFAILED"), "Session", entity.SessionId)));
        }

        return Result.Success();
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
                return Result.Failure(
                    Error.OperationFailed(string.Format(
                        Message.GetResource("ErrorMessages", "LOG_DELETE_OPERATIONFAILED"), "Session", "-")));
            }
        }
        catch (DbUpdateException ex)
        {
            return Result.Failure(
                Error.OperationFailed(string.Format(
                    Message.GetResource("ErrorMessages", "LOG_DELETE_OPERATIONFAILED"), "Session", "-")));
        }

        return Result.Success();
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
