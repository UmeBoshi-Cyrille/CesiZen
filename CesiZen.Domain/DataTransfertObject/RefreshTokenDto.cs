namespace CesiZen.Domain.DataTransfertObject;

public class RefreshTokenDto
{
    public string? Token { get; set; }
    public DateTime? ExpirationTime { get; set; }
}
