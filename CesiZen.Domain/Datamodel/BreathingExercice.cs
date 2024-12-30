using CesiZen.Domain.Enum;

namespace CesiZen.Domain.Datamodel;

public class BreathingExercice : AId
{
    public int UserId { get; }

    public ExerciceType Exercice { get; set; }

    public Timer Timer { get; set; }

    public BreathingExercice() { }
}
