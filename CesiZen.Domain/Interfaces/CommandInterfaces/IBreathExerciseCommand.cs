using CesiZen.Domain.Datamodel;
using CesiZen.Domain.DataTransfertObject;

namespace CesiZen.Domain.Interfaces;

public interface IBreathExerciseCommand : ICommand<BreathExerciseMinimumDto, BreathExercise>
{
}
