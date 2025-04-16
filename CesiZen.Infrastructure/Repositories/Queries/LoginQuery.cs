using CesiZen.Domain.BusinessResult;
using CesiZen.Domain.Datamodel;
using CesiZen.Domain.DataTransfertObject;
using CesiZen.Domain.Interfaces;
using CesiZen.Domain.Mapper;
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

    public async Task<IResult<AuthenticationLoginDto>> GetByUserId(int userId)
    {
        var login = await context.Logins
                           .AsNoTracking()
                           .Where(x => x.UserId == userId)
                           .Select(x => x.MapAuthenticationLoginDto())
                           .FirstOrDefaultAsync();

        if (login == null)
        {
            return Result<AuthenticationLoginDto>.Failure(UserErrors.LogNotFound(nameof(userId)));
        }

        return Result<AuthenticationLoginDto>.Success(login);
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

    public async Task<IResult> CheckEmail(string email)
    {
        var exist = await context.Logins
                            .AsNoTracking()
                            .AnyAsync(x => x.Email == email);

        if (exist)
        {
            return Result<Login>.Failure(UserErrors.LogNotUnique(email));
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
                    ResourceMessages.GetResource("ErrorMessages", "LOG_GETONE_NOTFOUND"), "Login", email)));
        }

        return Result<ResetPassword>.Success(resetPassword);
    }
}