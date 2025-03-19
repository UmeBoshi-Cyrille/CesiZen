using CesiZen.Domain.DataTransfertObject;

namespace CesiZen.Domain.Interfaces;

public interface IBreathExerciseQueryService
{
    Task<IResult<BreathExerciseDto>> GetByIdAsync(string id);
    Task<IResult<List<BreathExerciseDto>>> GetAllByIdAsync(string userId);
}
