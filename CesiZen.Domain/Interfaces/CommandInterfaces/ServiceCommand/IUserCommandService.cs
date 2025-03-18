using CesiZen.Domain.Datamodel;

namespace CesiZen.Domain.Interface;

public interface IUserCommandService : ICommandServiceInterface<User>
{
    Task<IResult> UpdateUserName(string id, string userName);
}
