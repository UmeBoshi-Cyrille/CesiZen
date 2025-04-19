namespace CesiZen.Domain.Interfaces;

public interface ILoginCommandService
{
    Task<IResult> UpdateEmail(int userId, string email);
}
