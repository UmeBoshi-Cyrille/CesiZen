using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CesiZen.Domain.Datamodel;

public class User
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }

    [MaxLength(50)]
    [MinLength(2)]
    [RegularExpression(@"^[a-zA-Z]+$")]
    public string Firstname { get; set; } = string.Empty;

    [MaxLength(50)]
    [MinLength(2)]
    [RegularExpression(@"^[a-zA-Z]+$")]
    public string Lastname { get; set; } = string.Empty;

    [MaxLength(50)]
    [MinLength(2)]
    [RegularExpression(@"^[a-zA-Z]+$")]
    public string Username { get; set; } = string.Empty;

    public DateTime CreatedAt { get; set; }

    public DateTime? LastLoginAt { get; set; }

    public DateTime? UpdatedAt { get; set; }

    public bool IsActive { get; set; }

    public string Role { get; set; } = string.Empty;

    public Login? Login { get; set; }

    public RefreshToken? RefreshToken { get; set; }

    public Session? Session { get; set; }

    public ICollection<BreathExercise>? BreathExercises { get; set; }
}
