namespace CesiZen.Domain.DataTransfertObject;

public class AuthenticateResponseDto
{
    public bool IsLoggedIn { get; set; } = false;
    public string? Token { get; set; }
}
