using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace CesiZen.Domain.DataTransfertObject;

public class NewArticleDto
{
    [MaxLength(255, ErrorMessage = "255 caractères autorisés")]
    [MinLength(2, ErrorMessage = "2 caractères minimum requis")]
    [RegularExpression(@"^\S[a-zA-ZÀ-ÿ\s !^()-_;'’:"",.?\p{L}+]+\S$", ErrorMessage = "Caractères présents non autorisés.")]
    [DefaultValue("Titre")]
    [Required(ErrorMessage = "Le titre est obligatoire")]
    public string Title { get; set; } = string.Empty;

    [MaxLength(3000, ErrorMessage = "3000 caractères autorisés")]
    [RegularExpression(@"^\S[a-zA-ZÀ-ÿ\s !^()%-_;'’:"",.?\p{L}+]+$", ErrorMessage = "Caractères présents non autorisés.")]
    [Required(ErrorMessage = "La description est obligatoire")]
    public string Description { get; set; } = string.Empty;

    [MaxLength(54, ErrorMessage = "54 caractères autorisés")]
    [MinLength(2, ErrorMessage = "2 caractères minimum requis")]
    [RegularExpression(@"^[a-zA-ZÀ-ÿ]+$", ErrorMessage = "Caractères présents non autorisés")]
    [Required(ErrorMessage = "Le champs auteur est obligatoire.")]
    [DefaultValue("Auteur")]
    public string Author { get; set; } = string.Empty;

    [MaxLength(30000, ErrorMessage = "30000 caractères autorisés")]
    [Required(ErrorMessage = "Le contenu est obligatoire")]
    public string Content { get; set; } = string.Empty;

    [MaxLength(200, ErrorMessage = "200 caractères autorisés")]
    //[MinLength(7, ErrorMessage = "7 caractères minimum requis")]
    public string ImagePath { get; set; } = string.Empty;
    public ICollection<ImageDto>? Images { get; set; }
    public List<int>? Categories { get; set; }
}
