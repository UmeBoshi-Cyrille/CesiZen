using CesiZen.Domain.BusinessResult;
using CesiZen.Domain.Interface;
using CesiZen.Domain.Interfaces;
using CesiZen.Infrastructure.DatabaseContext;
using Microsoft.EntityFrameworkCore;

namespace CesiZen.Infrastructure.Repository;

public class SessionQuery : AbstractRepository, ISessionQuery
{
    public SessionQuery(
        MongoDbContext context)
        : base(context)
    {
    }

    public async Task<IResult<int>> GetId(int id)
    {
        var result = await context.Sessions
                .AsNoTracking()
                .Where(context => context.UserId == id)
                .Select(p => p.Id)
                .FirstOrDefaultAsync();

        if (result == null)
        {
            return Result<int>.Failure(
                Error.NotFound(string.Format(
                    Message.GetResource("ErrorMessages", "LOG_GETONE_NOTFOUND"), "RefreshToken", id)));
        }

        return Result<int>.Success(result);
    }
}
