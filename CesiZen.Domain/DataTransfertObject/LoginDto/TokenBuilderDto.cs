namespace CesiZen.Domain.DataTransfertObject;

public class TokenBuilderDto
{
    public string SessionId { get; set; } = string.Empty;
    public string TokenId { get; set; } = string.Empty;
    public int ExpirationTime { get; set; }
    public int UserId { get; set; }
}
