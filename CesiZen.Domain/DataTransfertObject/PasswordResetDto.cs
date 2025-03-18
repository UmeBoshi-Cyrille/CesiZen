namespace CesiZen.Domain.DataTransfertObject;

public record PasswordResetDto
{
    public string Token { get; set; }
    public string NewPassword { get; set; }
    public string ConfirmPassword { get; set; }
}
