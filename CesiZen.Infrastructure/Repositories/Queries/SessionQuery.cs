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

    public async Task<IResult<string>> GetId(int Id)
    {
        var result = await context.Sessions
                .AsNoTracking()
                .Where(context => context.UserId == id)
                .Select(p => p.Id)
                .FirstOrDefaultAsync();

        if (result == null)
        {
            return Result<string>.Failure(SessionErrors.LogNotFound(id));
        }

        return Result<string>.Success(result);
    }
}
