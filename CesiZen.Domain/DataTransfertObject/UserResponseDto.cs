namespace CesiZen.Domain.DataTransfertObject;

public class UserResponseDto
{
    public int Id { get; set; }

    public DateTime CreatedAt { get; set; }

    public string Username { get; set; } = string.Empty;

    public string Role { get; set; } = string.Empty;

    public bool IsActive { get; set; }
}
