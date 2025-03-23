using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;

namespace CesiZen.Domain.Datamodel;

public class Login
{
    public int Id { get; set; }

    [Required(ErrorMessage = "Email is Required")]
    [EmailAddress(ErrorMessage = "Invalid Email format")]
    [RegularExpression(@"^[^@\s]+@[^@\s]+\.[^@\s]{2,3}$",
        ErrorMessage = "Invalid Email format")]

    public string Email { get; set; } = string.Empty;

    public bool EmailVerified { get; set; } = false;

    [MaxLength(150)]
    [MinLength(12)]
    [Required(ErrorMessage = "Invalid Password")]
    [RegularExpression(@"^(?=.*[!@#$%^&*()_+\-=\[\]{};':""\\|,.<>\/?])(?=.*[a-zA-Z])(?=.*\d)(?=.{12,})[^\s]+$",
        ErrorMessage = "Invalid password format")]
    public string Password { get; set; } = string.Empty;

    public string Salt { get; set; } = string.Empty;

    public string? PasswordResetToken { get; set; }

    public DateTime? PasswordResetTokenExpiry { get; set; }

    public string? EmailVerificationToken { get; set; }

    public int AccessFailedCount { get; set; }

    public bool IsLocked { get; set; }

    [DataType(DataType.DateTime)]
    public DateTime? LockoutEndTime { get; set; }

    public string UserId { get; set; } = string.Empty;

    [JsonIgnore]
    public User? User { get; set; }
}
