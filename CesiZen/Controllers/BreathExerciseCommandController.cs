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

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] BreathExerciseDto dto)
    {
        var result = exerciseCommandService.Insert(dto).Result;

        return result.Match<ActionResult>(
            success: () => CreatedAtAction(
                nameof(BreathExerciseQueryController.GetExercise),
                nameof(BreathExerciseQueryController),
                new { title = dto.Title, exercise = dto }),
            failure: error => BadRequest(new { error })
        );
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update([FromBody] BreathExerciseDto dto)
    {
        var result = await exerciseCommandService.Update(dto);

        var message = "Ok";
        return result.Match<IActionResult>(
            success: () => Ok(new { message }),
            failure: error => BadRequest(new { error })
        );
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(string id)
    {
        var result = await exerciseCommandService.Delete(id);

        var message = "Ok";

        return result.Match<IActionResult>(
            success: () => Ok(new { message }),
            failure: error => BadRequest(new { error })
        );
    }
}
