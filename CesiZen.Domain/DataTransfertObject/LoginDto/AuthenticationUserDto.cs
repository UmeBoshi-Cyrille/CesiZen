namespace CesiZen.Domain.DataTransfertObject;

public record AuthenticationUserDto
{
    public int Id { get; set; }
    public string Username { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; }
    public bool IsActive { get; set; }
    public string Role { get; set; } = string.Empty;
    public AuthenticationLoginDto? Login { get; set; }
    public RefreshTokenDto? RefreshToken { get; set; }
    public string? SessionId { get; set; }
}
