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

    public async Task<IResult<Login>> GetByUserId(int userId)
    {
        var login = await context.Logins
                           .AsNoTracking()
                           .FirstOrDefaultAsync(x => x.UserId == userId);

        if (login == null)
        {
            return Result<Login>.Failure(UserErrors.LogNotFound(nameof(userId)));
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

    public async Task<IResult<ResetPassword>> GetResetPassword(string email, string token)
    {
        var resetPasswords = await context.Logins
                           .AsNoTracking()
                           .Select(x => x.ResetPasswords!.Where(t => t.ResetToken == token))
                           .FirstOrDefaultAsync();

        var resetPassword = resetPasswords!.FirstOrDefault(t => t.ResetToken == token);

        if (resetPassword == null)
        {
            return Result<ResetPassword>.Failure(
                Error.NotFound(string.Format(
                    Message.GetResource("ErrorMessages", "LOG_GETONE_NOTFOUND"), "Login", email)));
        }

        return Result<ResetPassword>.Success(resetPassword);
    }
}