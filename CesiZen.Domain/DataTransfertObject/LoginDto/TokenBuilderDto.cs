namespace CesiZen.Domain.DataTransfertObject;

public class TokenBuilderDto
{
    public string SessionId { get; set; }
    public string TokenId { get; set; }
    public int ExpirationTime { get; set; }
    public int UserId { get; set; }
}
