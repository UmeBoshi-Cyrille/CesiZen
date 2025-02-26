namespace CesiZen.Domain.DataTransfertObject;

public class JwtSettings
{
    public string? SecretKey { get; set; }
    public string? RefreshSecret { get; set; }
    public string? Issuer { get; set; }
    public string? Audience { get; set; }
    public string? Subject { get; set; }
    public int ExpirationMinutes { get; set; }
}
