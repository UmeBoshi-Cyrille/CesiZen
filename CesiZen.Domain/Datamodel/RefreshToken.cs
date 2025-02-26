using CesiZen.Domain.Datamodel;

public class RefreshToken
{
    public int Id { get; set; }
    public string Token { get; set; }
    public DateTime ExpirationTime { get; set; }
    public int UserId { get; set; }
    public User? User { get; set; }
}