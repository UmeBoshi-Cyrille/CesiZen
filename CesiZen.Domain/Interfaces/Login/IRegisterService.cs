using CesiZen.Domain.DataTransfertObject;
using CesiZen.Domain.Interfaces;

namespace CesiZen.Domain.Interfaces;

public interface IRegisterService
{
    Task<IResult> Register(UserDto user);
}
