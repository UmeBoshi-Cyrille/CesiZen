using CesiZen.Domain.DataTransfertObject;

namespace CesiZen.Domain.Interfaces;

public interface IBreathExerciseQueryService
{
    Task<IResult<BreathExerciseDto>> GetByIdAsync(int id);
    Task<IResult<List<BreathExerciseMinimumDto>>> GetAllByIdAsync(int userId);
}
