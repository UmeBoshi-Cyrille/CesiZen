using CesiZen.Domain.BusinessResult;
using CesiZen.Domain.Datamodel;
using CesiZen.Domain.Interfaces;
using CesiZen.Infrastructure.DatabaseContext;
using Microsoft.EntityFrameworkCore;

namespace CesiZen.Infrastructure.Repositories;

public class LoginQuery : AbstractRepository, ILoginQuery
{
    public LoginQuery(MongoDbContext context) : base(context)
    {
    }

    public async Task<IResult<Login>> GetByEmail(string email)
    {
        var login = await context.Logins
                           .AsNoTracking()
                           .FirstOrDefaultAsync(x => x.Email == email);

        if (login == null)
        {
            return Result<Login>.Failure(
                Error.NotFound(string.Format(
                    Message.GetResource("ErrorMessages", "LOG_GETONE_NOTFOUND"), "Login", email)));
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
            return Result<Login>.Failure(
                Error.NotFound(string.Format(
                    Message.GetResource("ErrorMessages", "LOG_GETONE_NOTFOUND"), "Login", userId)));
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
            return Result<Login>.Failure(
                Error.NotFound(string.Format(
                    Message.GetResource("ErrorMessages", "LOG_GETONE_NOTFOUND"), "Login", token)));
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
            return Result.Failure(
                Error.NotUnique(string.Format(
                    Message.GetResource("ErrorMessages", "LOG_CHECK_UNICITY_CONSTRAINT"), "Email", providedEmail)));
        }

        return Result.Success();
    }
}