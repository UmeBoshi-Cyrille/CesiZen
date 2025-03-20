using CesiZen.Domain.Enum;
using System.ComponentModel.DataAnnotations;

namespace CesiZen.Domain.DataTransfertObject;

public class BreathExerciseDto
{
    public string Id { get; set; } = string.Empty;

    [MaxLength(100)]
    public string Title { get; set; } = string.Empty;

    [Range(1, 600)]
    public int? Time { get; set; }

    public ExerciceType ExerciseType { get; set; }
}
