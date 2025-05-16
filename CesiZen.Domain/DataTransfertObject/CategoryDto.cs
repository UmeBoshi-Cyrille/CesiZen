using System.ComponentModel.DataAnnotations;

namespace CesiZen.Domain.DataTransfertObject;

public class CategoryDto
{
    public int Id { get; set; }

    [MaxLength(255, ErrorMessage = "255 caractères autorisés")]
    [Required(ErrorMessage = "Le champs nom est obligatoire")]
    [RegularExpression(@"^[a-zA-ZÀ-ÿ-]+$", ErrorMessage = "Caractères présents non autorisés")]
    public string Name { get; set; } = string.Empty;

    [MaxLength(255)]
    public string ImagePath { get; set; } = string.Empty;

    public string Alternative { get; set; } = string.Empty;
}
