using CesiZen.Domain.BusinessResult;
using CesiZen.Domain.Datamodel;
using CesiZen.Domain.Interface;
using CesiZen.Infrastructure.DatabaseContext;
using Microsoft.EntityFrameworkCore;

namespace CesiZen.Infrastructure.Repository;

public class UserQuery : AbstractRepository, IUserQuery
{
    public UserQuery(MongoDbContext context) : base(context)
    {
    }

    public async Task<IResult<IEnumerable<User>>> GetAllAsync()
    {
        var users = await context.Users.AsNoTracking().ToListAsync();

        if (!users.Any())
        {
            return Result<IEnumerable<User>>.Failure(
                Error.NotFound(string.Format(
                    Message.GetResource("ErrorMessages", "LOG_GET_MULTIPLE_NOTFOUND"), "Users")));
        }

        return Result<IEnumerable<User>>.Success(users);
    }

    public async Task<IResult<IEnumerable<User>>> GetAllPaginatedAsync(int pageNumber, int pageSize)
    {
        var users = await context.Users
                .AsNoTracking()
                .OrderBy(x => x.UserName)
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

        if (!users.Any())
        {
            return Result<IEnumerable<User>>.Failure(
                Error.NotFound(string.Format(
                    Message.GetResource("ErrorMessages", "LOG_GET_MULTIPLE_NOTFOUND"), "Users")));
        }

        return Result<IEnumerable<User>>.Success(users);
    }

    public async Task<IResult<User>> GetByIdAsync(int id)
    {
        var user = await context.Users.FindAsync(id);

        if (user == null)
        {
            return Result<User>.Failure(
                Error.NotFound(string.Format(
                    Message.GetResource("ErrorMessages", "LOG_GETONE_NOTFOUND"), "User", id)));
        }

        return Result<User>.Success(user);
    }

    public async Task<IResult<User>> GetByUsername(string username)
    {
        User? user = await context.Users
                    .AsNoTracking()
                    .Include(x => x.Login)
                    .FirstOrDefaultAsync(x => x.UserName == username);

        if (user == null)
        {
            return Result<User>.Failure(
                Error.NotFound(string.Format(
                    Message.GetResource("ErrorMessages", "LOG_GETONE_NOTFOUND"), "User", username)));
        }

        return Result<User>.Success(user);
    }
}
