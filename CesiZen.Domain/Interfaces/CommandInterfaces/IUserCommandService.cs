using CesiZen.Domain.DataTransfertObject;

namespace CesiZen.Domain.Interfaces;

public interface IUserCommandService : ICommandServiceInterface<UserDto>
{
    Task<IResult> UpdateUserName(int Id, string userName);

    Task<IResult> ActivationAsync(AccountActivationDto dto);
}
