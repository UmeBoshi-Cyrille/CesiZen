using CesiZen.Domain.Datamodel;

namespace CesiZen.Domain.Interface;

public interface IUserCommandService : ICommandServiceInterface<User>
{
    Task<IResult> UpdatePostalCode(int id, uint postalCode);

    Task<IResult> UpdateUserName(int id, string userName);
}
