using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace CesiZen.Domain.DataTransfertObject;

public record PasswordResetDto
{
    [MaxLength(50, ErrorMessage = "50 caractères autorisés")]
    [MinLength(12, ErrorMessage = "12 caractères minimum requis")]
    [DefaultValue("password")]
    [Required(ErrorMessage = "Le champs Mot de passe obligatoire")]
    [RegularExpression(@"^(?=.*[!@#$%^&*()_+\-=\[\]{};':""\\|,.<>\/?])(?=.*[a-zA-Z])(?=.*\d)(?=.{12,})[^\s]+$",
        ErrorMessage = "Le mot de passe doit contenir au minimum 12 caractères, une lettre majuscule, une lettre minuscule, un chiffre et un caractère spécial.")]
    public string? CurrentPassword { get; set; }

    [MaxLength(50, ErrorMessage = "50 caractères autorisés")]
    [MinLength(12, ErrorMessage = "12 caractères minimum requis")]
    [DefaultValue("password")]
    [Required(ErrorMessage = "Le champs Mot de passe obligatoire")]
    [RegularExpression(@"^(?=.*[!@#$%^&*()_+\-=\[\]{};':""\\|,.<>\/?])(?=.*[a-zA-Z])(?=.*\d)(?=.{12,})[^\s]+$",
        ErrorMessage = "Le mot de passe doit contenir au minimum 12 caractères, une lettre majuscule, une lettre minuscule, un chiffre et un caractère spécial.")]
    public string? NewPassword { get; set; }

    [MaxLength(50, ErrorMessage = "50 caractères autorisés")]
    [MinLength(12, ErrorMessage = "12 caractères minimum requis")]
    [DefaultValue("password")]
    [Required(ErrorMessage = "Confirmation de mot de passe obligatoire")]
    [Compare("NewPassword", ErrorMessage = "La confirmation du mot de passe ne correspondent pas.")]
    public string? ConfirmPassword { get; set; }
}
