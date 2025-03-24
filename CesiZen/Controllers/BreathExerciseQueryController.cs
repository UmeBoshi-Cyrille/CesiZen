using CesiZen.Application.Authorization;
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

    /// <summary>
    /// Get breath exercices by user id
    /// </summary>
    /// <param name="userId">id provided by the client</param>
    /// <response code="200">data retrieved</response>
    /// <response code="404">Not Found</response>
    /// <response code="500">service unvalaible</response>
    /// <returns></returns>
    [HttpGet("exercises")]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [RoleAuthorization(Roles = "User")]
    public async Task<ActionResult<List<BreathExerciseDto>>> GetExercises([FromQuery] int userId)
    {
        var result = await exerciseService.GetAllByIdAsync(userId);

        return result.Match<ActionResult, List<BreathExerciseDto>>(
             success: value => Ok(new { value }),
             failure: error => NotFound(new { message = error.Message })
        );
    }

    /// <summary>
    /// Get breath exercice by id
    /// </summary>
    /// <param name="id">id provided by the client</param>
    /// <response code="200">data retrieved</response>
    /// <response code="404">Not Found</response>
    /// <response code="500">service unvalaible</response>
    /// <returns></returns>
    [HttpGet("exercise/{id}")]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [RoleAuthorization(Roles = "User")]
    public async Task<ActionResult<BreathExerciseDto>> GetExercise(int id)
    {
        var result = await exerciseService.GetByIdAsync(id);

        return result.Match<ActionResult, BreathExerciseDto>(
            success: value => Ok(new { value }),
            failure: error => NotFound(new { message = error.Message })
        );
    }
}
