using CesiZen.Domain.Enum;
using System.ComponentModel.DataAnnotations;

namespace CesiZen.Domain.DataTransfertObject;

public class NewBreathExerciseDto
{
    [MaxLength(100)]
    public string Title { get; set; } = string.Empty;

    [Range(1, 600)]
    public int Time { get; set; }

    public DateTime EditedAt { get; set; }

    public int userId { get; set; }

    public ExerciceType ExerciseType { get; set; }
}
