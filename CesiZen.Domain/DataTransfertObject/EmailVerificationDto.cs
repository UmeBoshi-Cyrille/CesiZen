namespace CesiZen.Domain.DataTransfertObject;

public class EmailVerificationDto
{
    public string Email { get; set; }

    public string EmailVerificationToken { get; set; }

    public bool EmailVerified { get; set; }
}