namespace CesiZen.Domain.DataTransfertObject;

public record PasswordResetRequestDto
{
    public string? Email { get; set; }
}
