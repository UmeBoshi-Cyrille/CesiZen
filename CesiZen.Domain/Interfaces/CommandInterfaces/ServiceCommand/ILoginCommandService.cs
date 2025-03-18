namespace CesiZen.Domain.Interface;

public interface ILoginCommandService
{
    Task UpdateEmail(int userId, string email);

    Task UpdatePassword(int userId, string password);
}
