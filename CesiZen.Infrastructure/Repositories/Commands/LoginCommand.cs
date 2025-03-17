using CesiZen.Domain.BusinessResult;
using CesiZen.Domain.Datamodel;
using CesiZen.Domain.DataTransfertObject;
using CesiZen.Domain.Interface;
using CesiZen.Infrastructure.DatabaseContext;
using Microsoft.EntityFrameworkCore;

namespace CesiZen.Infrastructure.Repositories;

public class LoginCommand : AbstractRepository, ILoginCommand
{
    public LoginCommand(MongoDbContext context) : base(context)
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
        }
        catch (DbUpdateException ex)
        {
            Result.Failure(
                Error.OperationFailed(string.Format(
                    Message.GetResource("ErrorMessages", "LOG_UPDATE_PROPERTY_OPERATIONFAILED"), "Login", "Email", $"Email: {dto.Email}")));
        }

        return Result.Success();
    }

    public async Task<IResult> UpdateEmail(string userId, string email)
    {
        try
        {
            await context.Logins
                .Where(x => x.UserId == userId)
                .ExecuteUpdateAsync(o => o.SetProperty(x => x.Email, email));
        }
        catch (DbUpdateException ex)
        {
            Result.Failure(
                Error.OperationFailed(string.Format(
                    Message.GetResource("ErrorMessages", "LOG_UPDATE_PROPERTY_OPERATIONFAILED"), "Login", "Email", $"Id: {userId}")));
        }

        return Result.Success();
    }

    public async Task<IResult> UpdatePassword(string userId, string password)
    {
        try
        {
            await context.Logins
               .Where(x => x.UserId == userId)
               .ExecuteUpdateAsync(o => o.SetProperty(x => x.Password, password));
        }
        catch (DbUpdateException ex)
        {
            Result.Failure(
                Error.OperationFailed(string.Format(
                    Message.GetResource("ErrorMessages", "LOG_UPDATE_PROPERTY_OPERATIONFAILED"), "Login", "Password", $"Id: {userId}")));
        }

        return Result.Success();
    }

    public async Task<IResult> ResetPassword(string token, string password)
    {
        try
        {

            var login = await context.Logins
                         .FirstOrDefaultAsync(t => t.EmailVerificationToken == token);

            if (login == null)
            {
                return Result.Failure(
                Error.NotFound(string.Format(
                    Message.GetResource("ErrorMessages", "CLIENT_NOTFOUND"), "Login")));
            }

            login.Password = password;
            login.PasswordResetToken = null;
            login.PasswordResetTokenExpiry = null;

            context.Logins.Update(login);
            await context.SaveChangesAsync();
        }
        catch (DbUpdateException ex)
        {
            Result.Failure(
                Error.OperationFailed(string.Format(
                    Message.GetResource("ErrorMessages", "LOG_UPDATE_PROPERTY_OPERATIONFAILED"), "Login", "Password", $"Token: {token}")));
        }

        return Result.Success();
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
        }
        catch (DbUpdateException ex)
        {
            Result.Failure(
                Error.OperationFailed(string.Format(
                    Message.GetResource("ErrorMessages", "LOG_UPDATE_PROPERTY_OPERATIONFAILED"), "Login", "Password Reset Token", $"Id: {login.Id}")));
        }

        return Result.Success();
    }

    public async Task<IResult> UpdateSalt(string userId, string salt)
    {
        try
        {
            await context.Logins
               .Where(x => x.UserId == userId)
               .ExecuteUpdateAsync(o => o.SetProperty(x => x.Salt, salt));
        }
        catch (DbUpdateException ex)
        {
            Result.Failure(
                Error.OperationFailed(string.Format(
                    Message.GetResource("ErrorMessages", "LOG_UPDATE_PROPERTY_OPERATIONFAILED"), "Login", "Salt", $"Id: {userId}")));
        }

        return Result.Success();
    }

    public async Task<IResult> UpdateLoginAttempsCount(Login login)
    {
        try
        {
            await context.Logins
                .Where(x => x.Id == login.Id)
                .ExecuteUpdateAsync(o => o.SetProperty(x => x.AccessFailedCount, login.AccessFailedCount));
        }
        catch (DbUpdateException ex)
        {
            Result.Failure(
                Error.OperationFailed(string.Format(
                    Message.GetResource("ErrorMessages", "LOG_UPDATE_PROPERTY_OPERATIONFAILED"), "Login", "AccessFailedCount", login.AccessFailedCount)));
        }

        return Result.Success();
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
        }
        catch (DbUpdateException ex)
        {
            Result.Failure(
                Error.OperationFailed(string.Format(
                    Message.GetResource("ErrorMessages", "LOG_UPDATE_OPERATIONFAILED"), "Login", "LoginAttemps properties", $"IsLocked: {login.IsLocked}")));
        }

        return Result.Success();
    }
}