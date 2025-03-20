namespace CesiZen.Domain.DataTransfertObject;

public class EmailVerificationDto
{
    public string Email { get; set; } = string.Empty;

    public string? EmailVerificationToken { get; set; } = string.Empty;

    public bool EmailVerified { get; set; }
}