namespace CesiZen.Domain.DataTransfertObject;

public record UserRequestDto
{
    public int Id { get; set; }

    public DateTime CreatedAt { get; set; }

    public DateTime? UpdatedAt { get; set; }

    public string Firstname { get; set; } = string.Empty;

    public string Lastname { get; set; } = string.Empty;

    public bool IsActive { get; set; }
}
