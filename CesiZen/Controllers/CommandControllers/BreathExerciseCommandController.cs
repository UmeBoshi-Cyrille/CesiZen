using CesiZen.Application.Authorization;
using CesiZen.Domain.BusinessResult;
using CesiZen.Domain.DataTransfertObject;
using CesiZen.Domain.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

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
    /// Creates a new breath exercise resource.
    /// </summary>
    /// <param name="dto">An object to provide containing the data required to create the breath exercise.</param>
    /// <response code="201">The breath exercise was successfully created.</response>
    /// <response code="400">The request was invalid or contained errors (e.g., validation failure).</response>
    /// <response code="500">An unexpected server error occurred while processing the request.</response>
    /// <returns>
    /// An <see cref="ActionResult"/> containing:
    /// - A 201 status code with the details of the newly created breath exercise if the operation succeeds.
    /// - A 400 status code if the request is invalid, such as missing required fields or failing validation checks.
    /// - A 500 status code if an unexpected server-side error occurs during the creation process.
    /// </returns>
    [HttpPost("create")]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [RoleAuthorization(Roles = "User, Admin")]
    public async Task<IActionResult> Create([FromBody] NewBreathExerciseDto dto)
    {
        var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

        if (string.IsNullOrEmpty(userIdClaim))
        {
            return Unauthorized(new { message = "User Id not found" });
        }

        if (!int.TryParse(userIdClaim, out var userId))
        {
            return BadRequest(new { message = "Invalid User Id format" });
        }

        dto.userId = userId;

        var result = await exerciseCommandService.Insert(dto);

        return result.Match<BreathExerciseMinimumDto, ActionResult>(
            success: createdExercise => CreatedAtAction(
                nameof(BreathExerciseQueryController.GetExercise),
                "BreathExerciseQuery",
                new { id = createdExercise.Id },
                new { data = createdExercise, message = result.Info.Message }),
            failure: error => BadRequest(new { message = Error.Alert, errors = error.Message })
        );
    }

    /// <summary>
    /// Updates the details of an existing breath exercise.
    /// </summary>
    /// <param name="id">The unique identifier of the breath exercise to update, provided by the client.</param>
    /// <param name="dto">An object containing the updated data to provide for the breath exercise.</param>
    /// <response code="200">The breath exercise was successfully updated.</response>
    /// <response code="400">The request was invalid or contained errors (e.g., validation failure).</response>
    /// <response code="404">The specified breath exercise was not found.</response>
    /// <response code="500">An unexpected server error occurred while processing the request.</response>
    /// <returns>
    /// An <see cref="ActionResult"/> containing:
    /// - A 200 status code with the updated breath exercise details if the operation succeeds.
    /// - A 400 status code if the request is invalid, such as missing required fields or failing validation checks.
    /// - A 404 status code if the specified breath exercise does not exist.
    /// - A 500 status code if an unexpected server-side error occurs during the update process.
    /// </returns>
    [HttpPut("{id:int}/update")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [RoleAuthorization(Roles = "User, Admin")]
    public async Task<IActionResult> Update(int id, [FromBody] BreathExerciseDto dto)
    {
        var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

        if (string.IsNullOrEmpty(userIdClaim))
        {
            return Unauthorized(new { message = "User Id not found" });
        }

        if (!int.TryParse(userIdClaim, out var userId))
        {
            return BadRequest(new { message = "Invalid User Id format" });
        }

        dto.userId = userId;

        var result = await exerciseCommandService.Update(dto);

        return result.Match<IActionResult>(
            success: () => Ok(new { message = result.Info.Message }),
            failure: error => error.Type switch
            {
                ErrorType.NotFound => NotFound(new { message = Error.Alert, errors = error.Message }),
                ErrorType.OperationFailed => BadRequest(new { message = Error.Alert, errors = error.Message }),
                _ => StatusCode(StatusCodes.Status500InternalServerError, new { message = Error.Alert, errors = error.Message })
            }
        );
    }

    /// <summary>
    /// Deletes a breath exercise resource identified by its unique ID.
    /// </summary>
    /// <param name="id">The unique identifier to provide of the breath exercise to be deleted.</param>
    /// <response code="204">The breath exercise was successfully deleted.</response>
    /// <response code="404">The specified breath exercise was not found.</response>
    /// <response code="400">The request was invalid or contained errors.</response>
    /// <response code="500">An unexpected server error occurred while processing the request.</response>
    /// <returns>
    /// An <see cref="ActionResult"/> containing:
    /// - A 204 status code if the breath exercise is successfully deleted and no content is returned.
    /// - A 404 status code if the specified breath exercise does not exist.
    /// - A 400 status code if the request is invalid (e.g., malformed ID).
    /// - A 500 status code if an unexpected server-side error occurs during the deletion process.
    /// </returns>
    [HttpDelete("{id:int}/delete")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [RoleAuthorization(Roles = "User, Admin")]
    public async Task<IActionResult> Delete(int id)
    {
        var result = await exerciseCommandService.Delete(id);

        return result.Match<IActionResult>(
            success: () => Ok(new { message = result.Info.Message }),
            failure: error => error.Type switch
            {
                ErrorType.BadRequest => BadRequest(new { message = Error.Alert, errors = error.Message }),
                _ => StatusCode(StatusCodes.Status500InternalServerError, new { message = Error.Alert, errors = error.Message })
            }
        );
    }
}
