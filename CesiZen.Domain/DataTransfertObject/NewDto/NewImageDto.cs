using System.ComponentModel.DataAnnotations;

namespace CesiZen.Domain.DataTransfertObject;

public class NewImageDto
{
    [MaxLength(100, ErrorMessage = "100 caractères autorisés")]
    [Required(ErrorMessage = "Le champs titre est obligatoire")]
    [RegularExpression(@"^\S[a-zA-Z !^()_;':"",.?\p{L}+]+\S$", ErrorMessage = "Caractères présents non autorisés.")]
    public string Title { get; set; } = string.Empty;

    [MaxLength(300, ErrorMessage = "300 caractères autorisés")]
    [Required(ErrorMessage = "Le champs alternantive est obligatoire")]
    [RegularExpression(@"^\S[a-zA-Z !^()_;':"",.?\p{L}+]+\S$", ErrorMessage = "Caractères présents non autorisés.")]
    public string Alternative { get; set; } = string.Empty;

    [MaxLength(100)]
    public string Path { get; set; } = string.Empty;
}
