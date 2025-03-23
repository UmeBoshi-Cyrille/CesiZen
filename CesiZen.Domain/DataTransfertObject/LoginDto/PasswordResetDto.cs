using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace CesiZen.Domain.DataTransfertObject;

public record PasswordResetDto
{
    [MaxLength(50)]
    [MinLength(12)]
    [Required(ErrorMessage = "Password is required")]
    public string? CurrentPassword { get; set; }

    [MaxLength(50)]
    [MinLength(12)]
    [DefaultValue("New password")]
    [Required(ErrorMessage = "Password is required")]
    public string? NewPassword { get; set; }

    [MaxLength(50)]
    [MinLength(12)]
    [DefaultValue("New password")]
    [Required(ErrorMessage = "Confirm Password is required")]
    [Compare("NewPassword", ErrorMessage = "Password and Confirmation Password do not match.")]
    public string? ConfirmPassword { get; set; }
}
