using CesiZen.Domain.BusinessResult;
using CesiZen.Domain.Datamodel;
using CesiZen.Domain.DataTransfertObject;
using CesiZen.Domain.Interfaces;
using CesiZen.Domain.Mapper;
using CesiZen.Infrastructure.DatabaseContext;
using Microsoft.EntityFrameworkCore;

namespace CesiZen.Infrastructure.Repositories;

public class UserQuery : AbstractRepository, IUserQuery
{
    public UserQuery(CesizenDbContext context) : base(context)
    {
    }

    public async Task<IResult<PagedResultDto<UserMinimumDto>>> SearchUsers(PageParametersDto parameters, string searchTerm)
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
                .Select(x => x.MapMinimumDto())
                .ToListAsync();

            var result = new PagedResultDto<UserMinimumDto>
            {
                Data = Users,
                TotalCount = totalCount,
                PageNumber = parameters.PageNumber,
                PageSize = parameters.PageSize
            };

            return Result<PagedResultDto<UserMinimumDto>>.Success(result);
        }
        catch (Exception ex)
        {
            return Result<PagedResultDto<UserMinimumDto>>.Failure(UserErrors.LogMultipleNotFound, ex.Message);
        }
    }

    public async Task<IResult<PagedResultDto<UserMinimumDto>>> GetAllAsync(int pageNumber, int pageSize)
    {
        var users = await context.Users
                .AsNoTracking()
                .OrderBy(x => x.Username)
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .Select(x => x.MapMinimumDto())
                .ToListAsync();

        if (!users.Any())
        {
            return Result<PagedResultDto<UserMinimumDto>>.Failure(UserErrors.LogMultipleNotFound);
        }

        var result = new PagedResultDto<UserMinimumDto>
        {
            Data = users,
            TotalCount = users.Count,
            PageNumber = pageNumber,
            PageSize = pageSize
        };

        return Result<PagedResultDto<UserMinimumDto>>.Success(result);
    }

    public async Task<IResult<UserDto>> GetByIdAsync(int id)
    {
        var user = await context.Users
            .AsNoTracking()
            .Where(x => x.Id == id)
            .Include(x => x.Login)
            .Select(x => x.MapDto())
            .FirstOrDefaultAsync();

        if (user == null)
        {
            return Result<UserDto>.Failure(UserErrors.LogNotFound(nameof(id)));
        }

        return Result<UserDto>.Success(user);
    }

    public async Task<IResult<UserDto>> GetByUsername(string username)
    {
        var user = await context.Users
                    .AsNoTracking()
                    .Where(x => x.Username == username)
                    .Include(x => x.Login)
                    .Select(x => x.MapDto())
                    .FirstOrDefaultAsync();

        if (user == null)
        {
            return Result<UserDto>.Failure(UserErrors.LogNotFound(username));
        }

        return Result<UserDto>.Success(user);
    }

    public async Task<IResult<User>> GetByIdentifier(string identifier, bool isEmail = false)
    {
        User user;

        if (isEmail)
        {
            user = await context.Users
                        .AsNoTracking()
                        .Include(x => x.Login)
                        .FirstOrDefaultAsync(x => x.Login.Email == identifier)!;
        }
        else
        {
            user = await context.Users
                    .AsNoTracking()
                    .Include(x => x.Login)
                    .FirstOrDefaultAsync(x => x.Username == identifier)!;
        }

        if (user == null)
        {
            return Result<User>.Failure(UserErrors.LogNotFound(identifier));
        }

        return Result<User>.Success(user);
    }

    public async Task<IResult<int>> GetUserId(string sessionId)
    {
        var userId = await context.Sessions
                                .Where(p => p.SessionId == sessionId)
                                .Select(p => p.UserId)
                                .FirstOrDefaultAsync();


        if (userId != 0)
        {
            return Result<int>.Success(userId);
        }

        return Result<int>.Failure(UserErrors.LogNotFound(sessionId));
    }
}
