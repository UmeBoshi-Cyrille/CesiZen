using CesiZen.Domain.Datamodel;

namespace CesiZen.Domain.Interfaces;

public interface IUserCommand : ICommandInterface<User>
{
    Task<IResult> UpdateUserName(int Id, string userName);

    Task<IResult> ActivationAsync(User entity);
}
