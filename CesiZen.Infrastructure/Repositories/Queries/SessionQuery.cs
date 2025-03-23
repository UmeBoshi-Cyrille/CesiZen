using CesiZen.Domain.BusinessResult;
using CesiZen.Domain.Interfaces;
using CesiZen.Infrastructure.DatabaseContext;
using Microsoft.EntityFrameworkCore;

namespace CesiZen.Infrastructure.Repositories;

public class SessionQuery : AbstractRepository, ISessionQuery
{
    public SessionQuery(
        CesizenDbContext context)
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

        if (result != 0)
        {
            return Result<int>.Success(result);
        }

        return Result<int>.Failure(SessionErrors.LogNotFound(nameof(id)));
    }
}
