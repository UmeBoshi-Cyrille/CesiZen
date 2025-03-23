using CesiZen.Domain.BusinessResult;
using CesiZen.Domain.Datamodel;
using CesiZen.Domain.DataTransfertObject;
using CesiZen.Domain.Interfaces;
using CesiZen.Infrastructure.DatabaseContext;
using Microsoft.EntityFrameworkCore;

namespace CesiZen.Infrastructure.Repositories;

public class LoginCommand : AbstractRepository, ILoginCommand
{
    public LoginCommand(CesizenDbContext context) : base(context)
    {
    }

    public async Task<IResult> UpdateEmailVerification(EmailVerificationDto dto)
    {
        try
        {
            await context.Logins
                .Where(x => x.Email == dto.Email)
                .ExecuteUpdateAsync(o => o
                    .SetProperty(x => x.EmailVerified, dto.EmailVerified)
                    .SetProperty(x => x.EmailVerificationToken, dto.EmailVerificationToken));

            return Result.Success(UserInfos.LogEmailVerified(dto.Email));
        }
        catch (DbUpdateException ex)
        {
            return Result.Failure(UserErrors.ClientEmailVerificationFailed, dto.Email, ex.Message);
        }
    }

    public async Task<IResult> UpdateEmail(int userId, string email)
    {
        try
        {
            await context.Logins
                .Where(x => x.UserId == userId)
                .ExecuteUpdateAsync(o => o.SetProperty(x => x.Email, email));

            return Result.Success(UserInfos.LogUpdateProperty("email", userId));
        }
        catch (DbUpdateException ex)
        {
            return Result.Failure(UserErrors.LogUpdatePropertyFailed("email", nameof(userId)), nameof(userId), ex.Message);
        }
    }

    public async Task<IResult> UpdatePassword(int userId, string password)
    {
        try
        {
            await context.Logins
               .Where(x => x.UserId == userId)
               .ExecuteUpdateAsync(o => o.SetProperty(x => x.Password, password));

            return Result.Success(UserInfos.LogUpdateProperty("password", userId));
        }
        catch (DbUpdateException ex)
        {
            return Result.Failure(UserErrors.LogUpdatePropertyFailed("password", nameof(userId)), nameof(userId), ex.Message);
        }
    }

    public async Task<IResult> ResetPassword(string token, string password)
    {
        try
        {
            var login = await context.Logins
                         .FirstOrDefaultAsync(t => t.EmailVerificationToken == token);

            if (login == null)
            {
                return Result.Failure(UserErrors.LogNotFound(nameof(login.Id)));
            }

            login.Password = password;
            login.PasswordResetToken = null;
            login.PasswordResetTokenExpiry = null;

            context.Logins.Update(login);
            await context.SaveChangesAsync();

            return Result.Success();
        }
        catch (DbUpdateException ex)
        {
            return Result.Failure(UserErrors.LogUpdatePropertyFailed("password", $"{token}"), token, ex.Message);
        }
    }

    public async Task<IResult> UpdateResetPasswordToken(Login login)
    {
        try
        {
            await context.Logins
               .Where(x => x.UserId == login.UserId)
               .ExecuteUpdateAsync(o => o
                    .SetProperty(x => x.PasswordResetToken, login.PasswordResetToken)
                    .SetProperty(x => x.PasswordResetTokenExpiry, login.PasswordResetTokenExpiry));

            return Result.Success();
        }
        catch (DbUpdateException ex)
        {
            return Result.Failure(UserErrors.LogUpdatePropertyFailed("PasswordResetToken", nameof(login.Id)), nameof(login.Id), ex.Message);
        }
    }

    public async Task<IResult> UpdateSalt(int userId, string salt)
    {
        try
        {
            await context.Logins
               .Where(x => x.UserId == userId)
               .ExecuteUpdateAsync(o => o.SetProperty(x => x.Salt, salt));

            return Result.Success();
        }
        catch (DbUpdateException ex)
        {
            return Result.Failure(UserErrors.LogUpdatePropertyFailed("Salt", nameof(userId)), nameof(userId), ex.Message);
        }
    }

    public async Task<IResult> UpdateLoginAttempsCount(Login login)
    {
        try
        {
            await context.Logins
                .Where(x => x.Id == login.Id)
                .ExecuteUpdateAsync(o => o.SetProperty(x => x.AccessFailedCount, login.AccessFailedCount));

            return Result.Success();
        }
        catch (DbUpdateException ex)
        {
            return Result.Failure(UserErrors.LogLoginAttempsCount(login.AccessFailedCount.ToString(), login.Email), login.Email, ex.Message);
        }
    }

    public async Task<IResult> UpdateLoginAttemps(Login login)
    {
        try
        {
            await context.Logins
                .Where(x => x.Id == login.Id)
                .ExecuteUpdateAsync(o => o
                .SetProperty(x => x.AccessFailedCount, login.AccessFailedCount)
                .SetProperty(x => x.IsLocked, login.IsLocked)
                .SetProperty(x => x.LockoutEndTime, login.LockoutEndTime));

            return Result.Success();
        }
        catch (DbUpdateException ex)
        {
            return Result.Failure(UserErrors.LogLoginAttempsReached(login.Email), login.Email, ex.Message);
        }
    }
}