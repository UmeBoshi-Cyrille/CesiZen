namespace CesiZen.Domain.DataTransfertObject;

public record EmailSenderDto
{
    public required string Email { get; set; }
    public string? Template { get; set; }
    public string? Subject { get; set; }
    public string? Token { get; set; }
}
