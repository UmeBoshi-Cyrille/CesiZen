using CesiZen.Domain.Datamodel;

namespace CesiZen.Domain.Interface;

public interface IUserCommand : ICommandInterface<User>
{
    Task<IResult> UpdateUserName(string id, string userName);

    Task<IResult> ActivationAsync(User entity);
}
