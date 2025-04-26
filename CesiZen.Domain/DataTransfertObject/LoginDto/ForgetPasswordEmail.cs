using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace CesiZen.Domain.DataTransfertObject;

public class ForgetPasswordEmail
{
    [DefaultValue("example@gmail.com")]
    [Required(ErrorMessage = "Le champs email est obligatoire.")]
    [RegularExpression(@"^[^@\s]+@[^@\s]+\.[^@\s]{2,3}$", ErrorMessage = "Format d'email invalide. [Respecter le format: example@gmail.com]")]
    public required string Email { get; set; } = string.Empty;
}
