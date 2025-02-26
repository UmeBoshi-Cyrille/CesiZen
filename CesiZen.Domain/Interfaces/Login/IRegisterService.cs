using CesiZen.Domain.DataTransfertObject;
using CesiZen.Domain.Interface;

namespace CesiZen.Domain.Interfaces;

public interface IRegisterService
{
    Task<IResult> Register(UserDto user);
}
