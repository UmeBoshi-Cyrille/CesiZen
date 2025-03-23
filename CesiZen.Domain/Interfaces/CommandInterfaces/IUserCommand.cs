using CesiZen.Domain.Datamodel;

namespace CesiZen.Domain.Interfaces;

public interface IUserCommand : ICommand<User>
{
    Task<IResult> UpdateUserName(int id, string userName);

    Task<IResult> ActivationAsync(User entity);
}
