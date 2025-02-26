using CesiZen.Domain.BusinessResult;
using CesiZen.Domain.Datamodel;
using CesiZen.Domain.Interface;
using CesiZen.Domain.Interfaces;
using CesiZen.Infrastructure.DatabaseContext;
using CesiZen.Infrastructure.Repository;
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
                entity.Id = session.Id;
                Update(entity);
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

    public async Task<IResult> Delete(int id)
    {
        try
        {
            var result = await context.Sessions
                 .Where(s => s.Id == id)
                 .ExecuteDeleteAsync();

            if (result <= 0)
            {
                return Result.Failure(
                    Error.OperationFailed(string.Format(
                        Message.GetResource("ErrorMessages", "LOG_DELETE_OPERATIONFAILED"), "Session", id)));
            }
        }
        catch (DbUpdateException ex)
        {
            return Result.Failure(
                Error.OperationFailed(string.Format(
                    Message.GetResource("ErrorMessages", "LOG_DELETE_OPERATIONFAILED"), "Session", id)));
        }

        return Result.Success();
    }

    private void Insert(Session entity)
    {
        context.Sessions.Add(entity);
    }

    private void Update(Session entity)
    {
        context.Sessions.Update(entity);
    }
}
