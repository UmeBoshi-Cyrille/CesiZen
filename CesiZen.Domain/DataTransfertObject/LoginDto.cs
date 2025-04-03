namespace CesiZen.Domain.DataTransfertObject;

public class LoginDto
{
    public int Id { get; set; }

    public string Email { get; set; } = string.Empty;

    public bool EmailVerified { get; set; }
}