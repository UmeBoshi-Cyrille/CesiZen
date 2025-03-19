namespace CesiZen.Domain.DataTransfertObject;

public record AccountActivationDto
{
    public string Id { get; set; }

    public bool IsActive { get; set; }

}
