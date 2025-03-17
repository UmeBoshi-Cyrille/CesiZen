using CesiZen.Domain.BusinessResult;
using CesiZen.Domain.Datamodel;
using CesiZen.Domain.Interface;
using CesiZen.Infrastructure.DatabaseContext;
using Microsoft.EntityFrameworkCore;

namespace CesiZen.Infrastructure.Repositories;

internal class UserCommand : AbstractRepository, IUserCommand
{
    public UserCommand(MongoDbContext context) : base(context)
    {
    }

    public async Task<IResult> Insert(User entity)
    {
        try
        {
            context.Users.Add(entity);
            context.Logins.Add(entity.Login);
            await context.SaveChangesAsync();
        }
        catch (DbUpdateException ex)
        {
            if (ex.InnerException?.Message.Contains("IX_Logins_Email") == true)
            {
                return Result.Failure(
                    Error.NotUnique(string.Format(
                    Message.GetResource("ErrorMessages", "LOG_CHECK_UNICITY_CONSTRAINT"), "User", entity.Login.Email)));
            }
        }
        catch (Exception ex)
        {
            return Result.Failure(
                Error.OperationFailed(string.Format(
                    Message.GetResource("ErrorMessages", "LOG_REGISTER_OPERATIONFAILED"), "User")));
        }

        return Result.Success(
                Info.Success(string.Format(
                    Message.GetResource("InfoMessages", "LOG_INSERT_SUCCESS"), "User")));
    }

    public async Task<IResult> Update(User entity)
    {
        var user = await context.Users.FindAsync(entity.Id);

        if (user == null)
        {
            return Result.Failure(
                Error.NotFound(string.Format(
                    Message.GetResource("ErrorMessages", "LOG_GETONE_NOTFOUND"), "User", entity.Login.Email)));
        }

        context.Users.Update(entity);

        try
        {
            await context.SaveChangesAsync();

            return Result.Success();
        }
        catch (DbUpdateException ex)
        {
            return Result.Failure(
                Error.OperationFailed(string.Format(
                    Message.GetResource("ErrorMessages", "LOG_UPDATE_OPERATIONFAILED"), "User", entity.Login.Email)));
        }
    }

    public async Task<IResult> UpdateUserName(string id, string userName)
    {
        var user = new User() { Id = id, UserName = userName };
        context.Attach(user);
        context.Entry(user).Property(p => p.UserName).IsModified = true;

        try
        {
            await context.SaveChangesAsync();

            return Result.Success();
        }
        catch (DbUpdateException ex)
        {
            return Result.Failure(
                Error.OperationFailed(string.Format(
                    Message.GetResource("ErrorMessages", "LOG_UPDATE_PROPERTY_OPERATIONFAILED"), "User", "UserName", userName)));
        }
    }

    public async Task<IResult> ActivationAsync(User entity)
    {
        context.Attach(entity);
        context.Entry(entity).Property(p => p.IsActive).IsModified = true;
        context.Entry(entity).Property(p => p.UpdatedAt).IsModified = true;

        try
        {
            await context.SaveChangesAsync();

            return Result.Success();
        }
        catch (DbUpdateException ex)
        {
            return Result.Failure(
                Error.OperationFailed(string.Format(
                    Message.GetResource("ErrorMessages", "LOG_UPDATE_PROPERTY_OPERATIONFAILED"), "User", "UserName")));
        }
    }

    public async Task<IResult> Delete(string id)
    {
        try
        {
            await context.Users
                .Where(x => x.Id == id)
                .ExecuteDeleteAsync();

            return Result.Success();
        }
        catch (DbUpdateException ex)
        {
            return Result.Failure(
                    Error.OperationFailed(string.Format(
                        Message.GetResource("ErrorMessages", "LOG_DELETE_OPERATIONFAILED"), "User", $"id {id}")));
        }
    }
}
