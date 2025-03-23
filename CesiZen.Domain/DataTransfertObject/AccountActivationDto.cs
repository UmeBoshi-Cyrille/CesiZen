namespace CesiZen.Domain.DataTransfertObject;

public record AccountActivationDto
{
    public int Id { get; set; } = string.Empty;

    public bool IsActive { get; set; }

}
