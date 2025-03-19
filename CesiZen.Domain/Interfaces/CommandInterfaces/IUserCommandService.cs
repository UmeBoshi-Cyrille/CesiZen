using CesiZen.Domain.DataTransfertObject;

namespace CesiZen.Domain.Interfaces;

public interface IUserCommandService : ICommandServiceInterface<UserDto>
{
    Task<IResult> UpdateUserName(string id, string userName);

    Task<IResult> ActivationAsync(UserDto dto);
}
