using CesiZen.Domain.DataTransfertObject;

namespace CesiZen.Domain.Interface;

public interface ILoginQueryService
{
    Task<string> Authenticate(AuthenticateRequestDto model);
}
