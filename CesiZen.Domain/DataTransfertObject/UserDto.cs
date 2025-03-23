using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace CesiZen.Domain.DataTransfertObject;

public class UserDto
{
    public int Id { get; set; }

    [MaxLength(54)]
    [MinLength(2)]
    [RegularExpression(@"^[a-zA-Z]+$",
        ErrorMessage = "Invalid characters (no special characters")]
    [Required(ErrorMessage = "FirstName is required")]
    public required string Firstname { get; set; }
    [MaxLength(54)]
    [MinLength(2)]
    [RegularExpression(@"^[a-zA-Z]+$",
        ErrorMessage = "Invalid characters (no special characters")]
    [Required(ErrorMessage = "LastName is required")]
    public required string Lastname { get; set; }

    [MaxLength(54)]
    [MinLength(3)]
    [RegularExpression(@"^[a-zA-Z]+$",
        ErrorMessage = "Invalid characters (no special characters")]
    public required string Username { get; set; }

    [DefaultValue("example@gmail.com")]
    [Required(ErrorMessage = "Email is Required")]
    [RegularExpression(@"^[^@\s]+@[^@\s]+\.[^@\s]{2,3}$",
        ErrorMessage = "Invalid Email format")]
    public required string Email { get; set; } = string.Empty;

    [MaxLength(50)]
    [MinLength(12)]
    [DefaultValue("password")]
    [Required(ErrorMessage = "Password is required")]
    [RegularExpression(@"^(?=.*[!@#$%^&*()_+\-=\[\]{};':""\\|,.<>\/?])(?=.*[a-zA-Z])(?=.*\d)(?=.{12,})[^\s]+$",
        ErrorMessage = "Password must be at least 12 characters long, contain at least one uppercase letter, one lowercase letter, one number and one special character.")]
    public required string Password { get; set; } = string.Empty;

    [MaxLength(50)]
    [MinLength(12)]
    [DefaultValue("password")]
    [Required(ErrorMessage = "Confirm Password is required")]
    [Compare("Password", ErrorMessage = "Password and Confirmation Password do not match.")]
    public required string ConfirmPassword { get; set; }
}
