using CesiZen.Domain.Datamodel;
using CesiZen.Domain.DataTransfertObject;

namespace CesiZen.Domain.Interfaces;

public interface IUserCommand : ICommand<UserMinimumDto, User>
{
    Task<IResult> UpdateUserName(int id, string userName);

    Task<IResult> ActivationAsync(AccountActivationDto dto);
}
