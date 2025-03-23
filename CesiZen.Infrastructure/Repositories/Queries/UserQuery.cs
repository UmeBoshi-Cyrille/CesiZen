using CesiZen.Domain.BusinessResult;
using CesiZen.Domain.Datamodel;
using CesiZen.Domain.DataTransfertObject;
using CesiZen.Domain.Interfaces;
using CesiZen.Infrastructure.DatabaseContext;
using Microsoft.EntityFrameworkCore;

namespace CesiZen.Infrastructure.Repositories;

public class UserQuery : AbstractRepository, IUserQuery
{
    public UserQuery(CesizenDbContext context) : base(context)
    {
    }

    public async Task<IResult<PagedResultDto<User>>> SearchUsers(PageParametersDto parameters, string searchTerm)
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

            var result = new PagedResultDto<User>
            {
                Data = Users,
                TotalCount = totalCount,
                PageNumber = parameters.PageNumber,
                PageSize = parameters.PageSize
            };

            return Result<PagedResultDto<User>>.Success(result);
        }
        catch (Exception ex)
        {
            return Result<PagedResultDto<User>>.Failure(UserErrors.LogMultipleNotFound, ex.Message);
        }
    }

    public async Task<IResult<PagedResultDto<User>>> GetAllAsync(int pageNumber, int pageSize)
    {
        var users = await context.Users
                .AsNoTracking()
                .OrderBy(x => x.Username)
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

        if (!users.Any())
        {
            return Result<PagedResultDto<User>>.Failure(UserErrors.LogMultipleNotFound);
        }

        var result = new PagedResultDto<User>
        {
            Data = users,
            TotalCount = users.Count,
            PageNumber = pageNumber,
            PageSize = pageSize
        };

        return Result<PagedResultDto<User>>.Success(result);
    }

    public async Task<IResult<User>> GetByIdAsync(int Id)
    {
        var user = await context.Users.FindAsync(id);

        if (user == null)
        {
            return Result<User>.Failure(UserErrors.LogNotFound(id));
        }

        return Result<User>.Success(user);
    }

    public async Task<IResult<User>> GetByUsername(string username)
    {
        User? user = await context.Users
                    .AsNoTracking()
                    .Include(x => x.Login)
                    .FirstOrDefaultAsync(x => x.Username == username);

        if (user == null)
        {
            return Result<User>.Failure(UserErrors.LogNotFound(username));
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
            return Result<string>.Failure(UserErrors.LogNotFound(sessionId));
        }

        return Result<string>.Success(userId);
    }
}
