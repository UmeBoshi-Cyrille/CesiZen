using CesiZen.Domain.DataTransfertObject;

namespace CesiZen.Domain.Interfaces;

public interface IBreathExerciseQueryService
{
    Task<IResult<BreathExerciseDto>> GetByIdAsync(int Id);
    Task<IResult<List<BreathExerciseDto>>> GetAllByIdAsync(string userId);
}
