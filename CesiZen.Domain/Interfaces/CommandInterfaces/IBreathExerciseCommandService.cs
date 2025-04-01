using CesiZen.Domain.DataTransfertObject;

namespace CesiZen.Domain.Interfaces;

public interface IBreathExerciseCommandService
{
    Task<IResult<BreathExerciseMinimumDto>> Insert(NewBreathExerciseDto entity);

    Task<IResult> Update(BreathExerciseDto entity);

    Task<IResult> Delete(int id);
}
