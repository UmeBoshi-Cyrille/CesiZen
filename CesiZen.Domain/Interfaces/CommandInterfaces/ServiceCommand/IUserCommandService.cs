using CesiZen.Domain.DataTransfertObject;

namespace CesiZen.Domain.Interface;

public interface IUserCommandService : ICommandServiceInterface<UserDto>
{
    Task<IResult> UpdateUserName(string id, string userName);
}
