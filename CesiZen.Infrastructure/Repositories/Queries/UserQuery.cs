using CesiZen.Domain.BusinessResult;
using CesiZen.Domain.Datamodel;
using CesiZen.Domain.DataTransfertObject;
using CesiZen.Domain.Interfaces;
using CesiZen.Infrastructure.DatabaseContext;
using Microsoft.EntityFrameworkCore;

namespace CesiZen.Infrastructure.Repositories;

public class UserQuery : AbstractRepository, IUserQuery
{
    public UserQuery(MongoDbContext context) : base(context)
    {
    }

    public async Task<IResult<PagedResult<User>>> SearchUsers(PageParameters parameters, string searchTerm)
    {
        try
        {
            var query = context.Users.AsQueryable();

            if (!string.IsNullOrWhiteSpace(searchTerm))
            {
                query = query.Where(a => a.Firstname.Contains(searchTerm) || a.Lastname.Contains(searchTerm));
            }

            var totalCount = await query.CountAsync();
            var Users = await query
                .OrderByDescending(a => a.CreatedAt)
                .Skip((parameters.PageNumber - 1) * parameters.PageSize)
                .Take(parameters.PageSize)
                .ToListAsync();

            var result = new PagedResult<User>
            {
                Data = Users,
                TotalCount = totalCount,
                PageNumber = parameters.PageNumber,
                PageSize = parameters.PageSize
            };

            return Result<PagedResult<User>>.Success(result);
        }
        catch (Exception ex)
        {
            return Result<PagedResult<User>>.Failure(
                Error.NotFound(string.Format(
                    Message.GetResource("ErrorMessages", "LOG_GET_MULTIPLE_NOTFOUND"), "Users")));
        }
    }

    public async Task<IResult<PagedResult<User>>> GetAllAsync(int pageNumber, int pageSize)
    {
        var users = await context.Users
                .AsNoTracking()
                .OrderBy(x => x.UserName)
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

        if (!users.Any())
        {
            return Result<PagedResult<User>>.Failure(
                Error.NotFound(string.Format(
                    Message.GetResource("ErrorMessages", "LOG_GET_MULTIPLE_NOTFOUND"), "Users")));
        }

        var result = new PagedResult<User>
        {
            Data = users,
            TotalCount = users.Count,
            PageNumber = pageNumber,
            PageSize = pageSize
        };

        return Result<PagedResult<User>>.Success(result);
    }

    public async Task<IResult<User>> GetByIdAsync(string id)
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

    public async Task<IResult<string>> GetUserId(string sessionId)
    {
        var userId = await context.Sessions
                                .Where(p => p.SessionId == sessionId)
                                .Select(p => p.UserId)
                                .FirstOrDefaultAsync();


        if (string.IsNullOrEmpty(userId))
        {
            return Result<string>.Failure(
                Error.NotFound(string.Format(
                    Message.GetResource("ErrorMessages", "LOG_GETONE_NOTFOUND"), "User", "-")));
        }

        return Result<string>.Success(userId);
    }
}
