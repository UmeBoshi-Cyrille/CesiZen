using CesiZen.Domain.Enum;
using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;

namespace CesiZen.Domain.Datamodel;

public class BreathExercise
{
    public int Id { get; set; }

    [MaxLength(100)]
    public string Title { get; set; } = string.Empty;

    [Range(1, 600)]
    public int? Time { get; set; }

    public ExerciceType ExerciseType { get; set; }

    public string UserId { get; set; } = string.Empty;

    [JsonIgnore]
    public User? User { get; set; }
}
