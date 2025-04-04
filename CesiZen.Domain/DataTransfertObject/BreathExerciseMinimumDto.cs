using CesiZen.Domain.Enum;
using System.ComponentModel.DataAnnotations;

namespace CesiZen.Domain.DataTransfertObject;

public class BreathExerciseMinimumDto
{
    public int Id { get; set; }

    [MaxLength(100)]
    public string Title { get; set; } = string.Empty;

    public DateTime EditedAt { get; set; }

    public ExerciceType ExerciseType { get; set; }
}
