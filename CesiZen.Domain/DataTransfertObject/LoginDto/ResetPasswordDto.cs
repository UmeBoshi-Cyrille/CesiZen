namespace CesiZen.Domain.DataTransfertObject;

public class ResetPasswordDto
{
    public int userId { get; set; }
    public DateTime? ExpirationTime { get; set; }
}
