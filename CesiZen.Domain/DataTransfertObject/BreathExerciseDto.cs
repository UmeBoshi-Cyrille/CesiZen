using CesiZen.Domain.Enum;
using System.ComponentModel.DataAnnotations;

namespace CesiZen.Domain.DataTransfertObject;

public class BreathExerciseDto
{
    public string Id { get; set; }

    [MaxLength(100)]
    public string Title { get; set; }

    [Range(1, 600)]
    public int? Time { get; set; }

    public ExerciceType ExerciseType { get; set; }
}
