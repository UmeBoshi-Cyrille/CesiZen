namespace CesiZen.Domain.DataTransfertObject;

public class AuthenticateResponseDto
{
    public UserResponseDto User { get; set; } = new();
    public bool IsLoggedIn { get; set; } = false;
    public string? Token { get; set; }
}
