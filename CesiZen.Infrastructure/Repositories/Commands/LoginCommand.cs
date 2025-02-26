using CesiZen.Domain.BusinessResult;
using CesiZen.Domain.Datamodel;
using CesiZen.Domain.DataTransfertObject;
using CesiZen.Domain.Interface;
using CesiZen.Infrastructure.DatabaseContext;
using Microsoft.EntityFrameworkCore;

namespace CesiZen.Infrastructure.Repository;

public class LoginCommand : AbstractRepository, ILoginCommand
{
    public LoginCommand(MongoDbContext context) : base(context)
    {
    }

    public async Task<IResult> UpdateLogin(EmailVerificationDto dto)
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

    public async Task<IResult> UpdateEmail(int userId, string email)
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

    public async Task<IResult> UpdatePassword(int userId, string password)
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

    public async Task<IResult> UpdateSalt(int userId, string salt)
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