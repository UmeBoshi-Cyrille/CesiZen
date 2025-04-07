using System.ComponentModel.DataAnnotations;

namespace CesiZen.Domain.DataTransfertObject;

public class CategoryDto
{
    public int Id { get; set; }

    [MaxLength(54)]
    public string Name { get; set; } = string.Empty;

    [MaxLength(54)]
    public string ImagePath { get; set; } = string.Empty;
}
