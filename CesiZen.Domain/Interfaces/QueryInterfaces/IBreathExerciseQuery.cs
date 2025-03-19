using CesiZen.Domain.Datamodel;

namespace CesiZen.Domain.Interfaces;

public interface IBreathExerciseQuery
{
    Task<IResult<BreathExercise>> GetByIdAsync(string id);

    Task<IResult<List<BreathExercise>>> GetAllByIdAsync(string userId);
}
