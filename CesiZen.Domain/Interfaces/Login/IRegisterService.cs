using CesiZen.Domain.DataTransfertObject;

namespace CesiZen.Domain.Interfaces;

public interface IRegisterService
{
    Task<IResult<MessageEventArgs>> Register(UserDto dto);
}
