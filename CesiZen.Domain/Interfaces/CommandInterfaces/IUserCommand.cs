using CesiZen.Domain.Datamodel;

namespace CesiZen.Domain.Interfaces;

public interface IUserCommand : ICommandInterface<User>
{
    Task<IResult> UpdateUserName(string id, string userName);

    Task<IResult> ActivationAsync(User entity);
}
