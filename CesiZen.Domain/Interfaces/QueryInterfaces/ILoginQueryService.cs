using CesiZen.Domain.Datamodel;

namespace CesiZen.Domain.Interface;

public interface ILoginQueryService
{
    Task<IResult<Login>> GetByUserId(int userId);

    Task<IResult<Login>> GetByEmail(string email);
}
