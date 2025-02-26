using CesiZen.Domain.Datamodel;
using CesiZen.Domain.DataTransfertObject;

namespace CesiZen.Domain.Interfaces;

public interface IPasswordService
{
    Authentifier HashPassword(string password, User? user = null);

    bool VerifyPassword(Login login, string providedPassword);
}
