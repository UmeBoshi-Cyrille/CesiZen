namespace CesiZen.Domain.DataTransfertObject;

public record AuthenticationLoginDto
{
    public int Id { get; set; }
    public string Email { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
    public string Salt { get; set; } = string.Empty;
    public int AccessFailedCount { get; set; }
    public bool AccountIsLocked { get; set; }
    public DateTime? LockoutEndTime { get; set; }
}
