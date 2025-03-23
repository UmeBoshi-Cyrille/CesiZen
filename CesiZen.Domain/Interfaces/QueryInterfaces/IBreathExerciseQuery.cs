using CesiZen.Domain.Datamodel;

namespace CesiZen.Domain.Interfaces;

public interface IBreathExerciseQuery
{
    Task<IResult<BreathExercise>> GetByIdAsync(int Id);

    Task<IResult<List<BreathExercise>>> GetAllByIdAsync(string userId);
}
