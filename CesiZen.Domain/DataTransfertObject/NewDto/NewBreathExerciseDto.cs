using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace CesiZen.Domain.DataTransfertObject;

public class NewBreathExerciseDto
{
    [MaxLength(54, ErrorMessage = "54 caractères autorisés")]
    [MinLength(2, ErrorMessage = "2 caractères minimum requis")]
    [RegularExpression(@"^\S[a-zA-Z !^()_;':"",.?\p{L}+]+\S$", ErrorMessage = "Caractères présents non autorisés.")]
    [DefaultValue("Titre")]
    [Required(ErrorMessage = "Le titre est obligatoire")]
    public string Title { get; set; } = string.Empty;

    [Range(1, 600)]
    public int Time { get; set; }

    public DateTime EditedAt { get; set; }

    public int userId { get; set; }

    public int ExerciseType { get; set; }
}
