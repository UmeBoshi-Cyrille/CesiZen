using CesiZen.Domain.Datamodel;

namespace CesiZen.Domain.Interface;

public interface ILoginQuery
{
    Task<IResult<Login>> GetByUserId(int userId);

    Task<IResult<Login>> GetByEmail(string email);

    Task<IResult> CheckEmail(string email);
}