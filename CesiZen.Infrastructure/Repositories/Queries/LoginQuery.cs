using CesiZen.Domain.BusinessResult;
using CesiZen.Domain.Datamodel;
using CesiZen.Domain.Interfaces;
using CesiZen.Infrastructure.DatabaseContext;
using Microsoft.EntityFrameworkCore;

namespace CesiZen.Infrastructure.Repositories;

public class LoginQuery : AbstractRepository, ILoginQuery
{
    public LoginQuery(CesizenDbContext context) : base(context)
    {
    }

    public async Task<IResult<Login>> GetByEmail(string email)
    {
        var login = await context.Logins
                           .AsNoTracking()
                           .FirstOrDefaultAsync(x => x.Email == email);

        if (login == null)
        {
            return Result<Login>.Failure(UserErrors.LogNotFound(email));
        }

        return Result<Login>.Success(login);
    }

    public async Task<IResult<Login>> GetByUserId(string userId)
    {
        var login = await context.Logins
                           .AsNoTracking()
                           .FirstOrDefaultAsync(x => x.UserId == userId);

        if (login == null)
        {
            return Result<Login>.Failure(UserErrors.LogNotFound(userId));
        }

        return Result<Login>.Success(login);
    }

    public async Task<IResult<Login>> GetByResetPasswordToken(string token)
    {
        var login = await context.Logins
                           .AsNoTracking()
                           .FirstOrDefaultAsync(x => x.PasswordResetToken == token);

        if (login == null)
        {
            return Result<Login>.Failure(UserErrors.LogNotFound(token));
        }

        return Result<Login>.Success(login);
    }

    public async Task<IResult> CheckEmail(string providedEmail)
    {
        var exist = await context.Logins
                            .AsNoTracking()
                            .AnyAsync(x => x.Email == providedEmail);

        if (exist)
        {
            return Result<Login>.Failure(UserErrors.LogNotUnique(providedEmail));
        }

        return Result.Success();
    }
}