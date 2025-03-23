using CesiZen.Domain.BusinessResult;
using CesiZen.Domain.Datamodel;
using CesiZen.Domain.Interfaces;
using CesiZen.Infrastructure.DatabaseContext;
using Microsoft.EntityFrameworkCore;

namespace CesiZen.Infrastructure.Repositories;

internal class UserCommand : AbstractRepository, IUserCommand
{
    public UserCommand(CesizenDbContext context) : base(context)
    {
    }

    public async Task<IResult> Insert(User entity)
    {
        try
        {
            context.Users.Add(entity);
            context.Logins.Add(entity.Login!);
            await context.SaveChangesAsync();

            return Result.Success(UserInfos.LogInsertionSucceeded(entity.Username!));
        }
        catch (DbUpdateException ex)
        {
            Error error = new();

            if (ex.InnerException?.Message.Contains("IX_Logins_Email") == true)
            {
                error = UserErrors.LogNotUnique(entity.Login!.Email);
            }

            return Result.Failure(error, entity.Login!.Email, ex.Message);
        }
        catch (Exception ex)
        {
            return Result.Failure(UserErrors.LogRegisterFailed(entity.Login!.Email), entity.Login.Email, ex.Message);
        }
    }

    public async Task<IResult> Update(User entity)
    {
        var user = await context.Users.FindAsync(entity.Id);

        if (user == null)
        {
            return Result.Failure(UserErrors.LogUpdateFailed(entity.Id));
        }

        context.Users.Update(entity);

        try
        {
            await context.SaveChangesAsync();

            return Result.Success(UserInfos.LogUpdateSucceeded(entity.Id));
        }
        catch (DbUpdateException ex)
        {
            return Result.Failure(UserErrors.LogUpdateFailed(entity.Id), nameof(entity.Id), ex.Message);
        }
    }

    public async Task<IResult> UpdateUserName(int id, string userName)
    {
        var user = new User() { Id = id, Username = userName, UpdatedAt = DateTime.Now };
        context.Attach(user);
        context.Entry(user).Property(p => p.Username).IsModified = true;
        context.Entry(user).Property(p => p.UpdatedAt).IsModified = true;

        try
        {
            await context.SaveChangesAsync();

            return Result.Success(UserInfos.LogUpdateProperty("Username", id));
        }
        catch (DbUpdateException ex)
        {
            return Result.Failure(UserErrors.LogUpdatePropertyFailed("Username", id), nameof(id), ex.Message);
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

            Info info;
            if (entity.IsActive)
                info = UserInfos.LogAccountEnabled(nameof(entity.Id));
            else
                info = UserInfos.LogAccountDisabled(nameof(entity.Id));

            return Result.Success(info);
        }
        catch (DbUpdateException ex)
        {
            return Result.Failure(UserErrors.LogUpdatePropertyFailed("IsActive", entity.Id), nameof(entity.Id), ex.Message);
        }
    }

    public async Task<IResult> Delete(int id)
    {
        try
        {
            await context.Users
                .Where(x => x.Id == id)
                .ExecuteDeleteAsync();

            return Result.Success(UserInfos.LogDeleteCompleted(id));
        }
        catch (DbUpdateException ex)
        {
            return Result.Failure(UserErrors.LogDeletionFailed(id), nameof(id), ex.Message);
        }
    }
}
