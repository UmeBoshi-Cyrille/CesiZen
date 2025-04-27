using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CesiZen.Domain.Datamodel;

public class Login
{
    public int Id { get; set; }
    public string Email { get; set; } = string.Empty;
    public bool EmailVerified { get; set; } = false;
    public string Password { get; set; } = string.Empty;
    public string Salt { get; set; } = string.Empty;
    public string? PasswordResetToken { get; set; }
    public DateTime? PasswordResetTokenExpiry { get; set; }
    public string? EmailVerificationToken { get; set; }
    public int AccessFailedCount { get; set; }
    public bool AccountIsLocked { get; set; }

    [DataType(DataType.DateTime)]
    public DateTime? LockoutEndTime { get; set; }
    public int ResetFailedCount { get; set; } = 0;
    public bool ResetIsLocked { get; set; }
    public DateTime? ResetLockoutEndTime { get; set; }
    public ICollection<ResetPassword>? ResetPasswords { get; set; }
    public int UserId { get; set; }

    [JsonIgnore]
    public User? User { get; set; }

    [NotMapped]
    public ResetPassword? ResetPassword { get; set; }
}
