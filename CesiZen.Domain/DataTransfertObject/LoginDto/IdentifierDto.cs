namespace CesiZen.Domain.DataTransfertObject;

public class IdentifierDto
{
    public string? UserName { get; set; }
    public string? Email { get; set; }
    public string Password { get; set; }
    public string HashSalt { get; set; }

    public IdentifierDto(string password, string hashSalt)
    {
        Password = password;
        HashSalt = hashSalt;
    }
}
