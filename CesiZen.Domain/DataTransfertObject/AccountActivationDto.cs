namespace CesiZen.Domain.DataTransfertObject;

public record AccountActivationDto
{
    public int Id { get; set; }

    public bool IsActive { get; set; }
}
