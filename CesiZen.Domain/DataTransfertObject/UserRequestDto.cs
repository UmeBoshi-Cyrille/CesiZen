namespace CesiZen.Domain.DataTransfertObject;

public record UserRequestDto
{
    public string Id { get; set; }

    public DateTime CreatedAt { get; set; }

    public DateTime? UpdatedAt { get; set; }

    public string Firstname { get; set; }

    public string Lastname { get; set; }

    public bool IsActive { get; set; }
}
