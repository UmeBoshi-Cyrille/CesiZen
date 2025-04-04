using CesiZen.Domain.DataTransfertObject;

namespace CesiZen.Domain.Interfaces;

public interface IUserCommandService : ICommandService<UserAccountDto>
{
    Task<IResult> UpdateUserName(int id, string userName);

    Task<IResult> ActivationAsync(AccountActivationDto dto);
}
