using CesiZen.Application.Authorization;
using CesiZen.Domain.BusinessResult;
using CesiZen.Domain.DataTransfertObject;
using CesiZen.Domain.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace CesiZen.Api.Controllers;

[ApiController]
[Route("/api/breath-exercises/command")]
public class BreathExerciseCommandController : ControllerBase
{
    private readonly IBreathExerciseCommandService exerciseCommandService;

    public BreathExerciseCommandController(
        IBreathExerciseCommandService exerciseCommandService)
    {
        this.exerciseCommandService = exerciseCommandService;
    }

    /// <summary>
    /// Create new breath exercise
    /// </summary>
    /// <param name="dto">data provided by the client</param>
    /// <response code="201">operation succeeded</response>
    /// <response code="400">Bad request</response>
    /// <response code="500">service unvalaible</response>
    /// <returns></returns>
    [HttpPost("create")]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [RoleAuthorization(Roles = "User")]
    public async Task<IActionResult> Create([FromBody] NewBreathExerciseDto dto)
    {
        var result = await exerciseCommandService.Insert(dto);

        return result.Match<BreathExerciseMinimumDto, ActionResult>(
            success: createdExercise => CreatedAtAction(
                nameof(BreathExerciseQueryController.GetExercise),
                "BreathExerciseQueryController",
                new { id = createdExercise.Id },
                new { message = result.Info.Message, exercise = createdExercise }),
            failure: error => BadRequest(new { message = error.Message })
        );
    }

    /// <summary>
    /// Update breath exercise data
    /// </summary>
    /// <param name="id">id provided by the client</param>
    /// <param name="dto">data provided by the client</param>
    /// <response code="200">operation succeeded</response>
    /// <response code="400">Bad request</response>
    /// <response code="500">service unvalaible</response>
    /// <returns></returns>
    [HttpPut("{id:int}/update")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [RoleAuthorization(Roles = "User")]
    public async Task<IActionResult> Update(int id, [FromBody] BreathExerciseDto dto)
    {
        var result = await exerciseCommandService.Update(dto);

        return result.Match<IActionResult>(
            success: () => Ok(new { message = result.Info.Message }),
            failure: error => BadRequest(new { message = error.Message })
        );
    }

    /// <summary>
    /// Delete breath exercise
    /// </summary>
    /// <param name="id">id provided by the client</param>
    /// <response code="200">operation succeeded</response>
    /// <response code="400">Bad request</response>
    /// <response code="500">service unvalaible</response>
    /// <returns></returns>
    [HttpDelete("{id:int}/delete")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [RoleAuthorization(Roles = "User")]
    public async Task<IActionResult> Delete(int id)
    {
        var result = await exerciseCommandService.Delete(id);

        return result.Match<IActionResult>(
            success: () => Ok(new { message = result.Info.Message }),
            failure: error => BadRequest(new { message = error.Message })
        );
    }
}
