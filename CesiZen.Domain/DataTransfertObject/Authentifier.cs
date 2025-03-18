namespace CesiZen.Domain.DataTransfertObject;

public class Authentifier
{
    public string Password { get; set; }
    public string HashSalt { get; set; }

    public Authentifier(string password, string hashSalt)
    {
        Password = password;
        HashSalt = hashSalt;
    }
}