using System.ComponentModel.DataAnnotations;

namespace CesiZen.Domain.DataTransfertObject;

public class NewCategoryDto
{
    [MaxLength(54, ErrorMessage = "54 caractères autorisés")]
    [Required(ErrorMessage = "Le champs nom est obligatoire")]
    [RegularExpression(@"^[a-zA-Z]+$", ErrorMessage = "Caractères présents non autorisés")]
    public string Name { get; set; } = string.Empty;

    [MaxLength(54)]
    public string ImagePath { get; set; } = string.Empty;
    public string Alternative { get; set; } = string.Empty;
}
