namespace CesiZen.Domain.Datamodel;

public class ResetPassword
{
    public int Id { get; set; }
    public string? ResetToken { get; set; }
    public DateTime? ExpirationTime { get; set; }
    public DateTime CreateAt { get; set; }
    public int LoginId { get; set; }
    public Login Login { get; set; } = new();
}
