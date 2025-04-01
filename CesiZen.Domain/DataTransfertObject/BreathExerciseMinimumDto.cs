using System.ComponentModel.DataAnnotations;

namespace CesiZen.Domain.DataTransfertObject;

public record BreathExerciseMinimumDto
{
    public int Id { get; set; }

    [MaxLength(100)]
    public string Title { get; set; } = string.Empty;
}
