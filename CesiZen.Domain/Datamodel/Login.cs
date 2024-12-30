namespace CesiZen.Domain.Datamodel;

public class Login : AId
{
    public int UserId { get; set; }

    public string Email { get; set; } = string.Empty;

    public string Password { get; set; } = string.Empty;
}
