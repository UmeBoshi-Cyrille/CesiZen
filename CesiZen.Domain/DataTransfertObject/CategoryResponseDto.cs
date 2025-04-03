using System.ComponentModel.DataAnnotations;

namespace CesiZen.Domain.DataTransfertObject;

public class CategoryResponseDto
{
    public int Id { get; set; }

    [MaxLength(54)]
    public string Name { get; set; } = string.Empty;
}
