using CesiZen.Domain.BusinessResult;
using CesiZen.Domain.DataTransfertObject;
using CesiZen.Domain.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace CesiZen.Api.Controllers;

[ApiController]
[Route("/api/[controller]")]
public class BreathExerciseQueryController : ControllerBase
{
    private readonly IBreathExerciseQueryService exerciseService;

    public BreathExerciseQueryController(IBreathExerciseQueryService exerciseService)
    {
        this.exerciseService = exerciseService;
    }

    [HttpGet("exercises")]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<List<BreathExerciseDto>>> GetExercises([FromQuery] string userId)
    {
        var result = await exerciseService.GetAllByIdAsync(userId);

        return result.Match<ActionResult, List<BreathExerciseDto>>(
             success: value => Ok(new { value }),
             failure: error => NotFound(new { message = error.Message })
        );
    }

    [HttpGet("exercise/{id}")]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<BreathExerciseDto>> GetExercise(string id)
    {
        var result = await exerciseService.GetByIdAsync(id);

        return result.Match<ActionResult, BreathExerciseDto>(
            success: value => Ok(new { value }),
            failure: error => NotFound(new { message = error.Message })
        );
    }
}
