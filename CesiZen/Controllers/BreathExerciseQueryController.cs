using CesiZen.Application.Authorization;
using CesiZen.Domain.BusinessResult;
using CesiZen.Domain.DataTransfertObject;
using CesiZen.Domain.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace CesiZen.Api.Controllers;

[ApiController]
[Route("/api/breath-exercise/query")]
public class BreathExerciseQueryController : ControllerBase
{
    private readonly IBreathExerciseQueryService exerciseService;

    public BreathExerciseQueryController(IBreathExerciseQueryService exerciseService)
    {
        this.exerciseService = exerciseService;
    }

    /// <summary>
    /// Retrieves a list of breath exercises associated with a specific user.
    /// </summary>
    /// <param name="userId">The unique identifier of the user to provide.</param>
    /// <response code="200">The breath exercises were successfully retrieved.</response>
    /// <response code="404">No breath exercises were found for the specified user.</response>
    /// <response code="500">An internal server error occurred while processing the request.</response>
    /// <returns>
    /// A list of breath exercises associated with a specific user.
    /// - A 200 status code with the list of breath exercises if found.
    /// - A 404 status code if no breath exercises are found for the specified user.
    /// - A 500 status code if there is a server error.
    /// </returns>
    [HttpGet("get")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [RoleAuthorization(Roles = "User")]
    public async Task<ActionResult<List<BreathExerciseMinimumDto>>> GetExercises([FromQuery] int userId)
    {
        var result = await exerciseService.GetAllByIdAsync(userId);

        return result.Match<ActionResult, List<BreathExerciseMinimumDto>>(
             success: value => Ok(new { value }),
             failure: error => NotFound(new { message = error.Message })
        );
    }

    /// <summary>
    /// Retrieves a specific breath exercise by its unique identifier.
    /// </summary>
    /// <param name="id">The unique identifier of the breath exercise to provide.</param>
    /// <response code="200">The breath exercise was successfully retrieved.</response>
    /// <response code="404">No breath exercise was found for the specified ID.</response>
    /// <response code="500">An internal server error occurred while processing the request.</response>
    /// <returns>
    /// The desired Breath exercise by its unique identifier.
    /// - A 200 status code with the breath exercise data if found.
    /// - A 404 status code if the breath exercise is not found.
    /// - A 500 status code if there is a server error.
    /// </returns>
    [HttpGet("{id:int}/details")]
    [ProducesResponseType(StatusCodes.Status200OK)]
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
