using CesiZen.Domain.BusinessResult;
using CesiZen.Domain.DataTransfertObject;
using CesiZen.Domain.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace CesiZen.Api.Controllers;

[ApiController]
[Route("/api/[controller]")]
public class BreathExerciseCommandController : ControllerBase
{
    private readonly IBreathExerciseCommandService exerciseCommandService;

    public BreathExerciseCommandController(
        IBreathExerciseCommandService exerciseCommandService)
    {
        this.exerciseCommandService = exerciseCommandService;
    }

    [HttpPost("create")]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> Create([FromBody] BreathExerciseDto dto)
    {
        var result = await exerciseCommandService.Insert(dto);

        return result.Match<ActionResult>(
            success: () => CreatedAtAction(
                nameof(BreathExerciseQueryController.GetExercise),
                nameof(BreathExerciseQueryController),
                new { title = dto.Title, exercise = dto, message = result.Info.Message }),
            failure: error => BadRequest(new { message = error.Message })
        );
    }

    [HttpPut("update/{id}")]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> Update([FromBody] BreathExerciseDto dto)
    {
        var result = await exerciseCommandService.Update(dto);

        return result.Match<IActionResult>(
            success: () => Ok(new { message = result.Info.Message }),
            failure: error => BadRequest(new { message = error.Message })
        );
    }

    [HttpDelete("delete/{id}")]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Delete(string id)
    {
        var result = await exerciseCommandService.Delete(id);

        return result.Match<IActionResult>(
            success: () => Ok(new { message = result.Info.Message }),
            failure: error => BadRequest(new { message = error.Message })
        );
    }
}
