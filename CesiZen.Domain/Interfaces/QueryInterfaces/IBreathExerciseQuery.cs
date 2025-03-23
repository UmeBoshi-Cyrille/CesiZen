using CesiZen.Domain.Datamodel;

namespace CesiZen.Domain.Interfaces;

public interface IBreathExerciseQuery
{
    Task<IResult<BreathExercise>> GetByIdAsync(int id);

    Task<IResult<List<BreathExercise>>> GetAllByIdAsync(int userId);
}
