using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace CesiZen.Domain.DataTransfertObject;

public class NewUserDto
{
    [MaxLength(50, ErrorMessage = "50 caractères autorisés")]
    [MinLength(2, ErrorMessage = "2 caractères minimum requis")]
    [RegularExpression(@"^[a-zA-Z]+$", ErrorMessage = "Caractères présents non autorisés")]
    [Required(ErrorMessage = "Le champs prénom est obligatoire.")]
    [DefaultValue("Prénom")]
    public required string Firstname { get; set; }

    [MaxLength(50, ErrorMessage = "50 caractères autorisés")]
    [MinLength(2, ErrorMessage = "2 caractères minimum requis")]
    [RegularExpression(@"^[a-zA-Z]+$", ErrorMessage = "Caractères présents non autorisés")]
    [Required(ErrorMessage = "Le champs nom est obligatoire.")]
    [DefaultValue("Nom")]
    public required string Lastname { get; set; }

    [MaxLength(50, ErrorMessage = "50 caractères autorisés")]
    [MinLength(3, ErrorMessage = "3 caractères minimum requis")]
    [RegularExpression(@"^[a-zA-Z]+$", ErrorMessage = "Caractères présents non autorisés")]
    [Required(ErrorMessage = "Le champs pseudonyme est obligatoire.")]
    [DefaultValue("Pseudonyme")]
    public required string Username { get; set; }

    [DefaultValue("example@gmail.com")]
    [Required(ErrorMessage = "Le champs email est obligatoire.")]
    [RegularExpression(@"^[^@\s]+@[^@\s]+\.[^@\s]{2,3}$", ErrorMessage = "Format d'email invalide. [Respecter le format: example@gmail.com]")]
    public required string Email { get; set; } = string.Empty;

    [MaxLength(50, ErrorMessage = "50 caractères autorisés")]
    [MinLength(12, ErrorMessage = "12 caractères minimum requis")]
    [DefaultValue("password")]
    [Required(ErrorMessage = "Le champs Mot de passe obligatoire")]
    [RegularExpression(@"^(?=.*[!@#$%^&*()_+\-=\[\]{};':""\\|,.<>\/?])(?=.*[a-zA-Z])(?=.*\d)(?=.{12,})[^\s]+$",
        ErrorMessage = "Le mot de passe doit contenir au minimum 12 caractères, une lettre majuscule, une lettre minuscule, un chiffre et un caractère spécial.")]
    public required string Password { get; set; } = string.Empty;

    [MaxLength(50, ErrorMessage = "50 caractères autorisés")]
    [MinLength(12, ErrorMessage = "12 caractères minimum requis")]
    [DefaultValue("password")]
    [Required(ErrorMessage = "Confirmation de mot de passe obligatoire")]
    [Compare("Password", ErrorMessage = "La confirmation du mot de passe ne correspondent pas.")]
    public required string ConfirmPassword { get; set; }
}
