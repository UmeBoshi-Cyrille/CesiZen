namespace CesiZen.Domain.DataTransfertObject;

public record AccountActivationDto
{
    public string Id { get; set; } = string.Empty;

    public bool IsActive { get; set; }

}
