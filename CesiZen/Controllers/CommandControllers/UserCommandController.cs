using CesiZen.Application.Authorization;
using CesiZen.Domain.BusinessResult;
using CesiZen.Domain.DataTransfertObject;
using CesiZen.Domain.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace CesiZen.Api.Controllers;

[ApiController]
[Route("api/users/command")]
public class UserCommandController : ControllerBase
{
    private readonly IUserCommandService userCommandService;
    private readonly ILoginCommandService loginCommandService;

    public UserCommandController(
        IUserCommandService userCommandService, ILoginCommandService loginCommandService)
    {
        this.userCommandService = userCommandService;
        this.loginCommandService = loginCommandService;
    }

    /// <summary>
    /// Updates the data of an existing user.
    /// </summary>
    /// <param name="dto">The updated user data to provide.</param>
    /// <response code="200">The user data was successfully updated.</response>
    /// <response code="400">The request was invalid or contained errors.</response>
    /// <response code="500">An internal server error occurred while processing the request.</response>
    /// <returns>
    /// An <see cref="ActionResult"/> containing:
    /// - A 200 status code if the update operation succeeds.
    /// - A 400 status code if the request is invalid (e.g., missing required fields or invalid data).
    /// - A 500 status code if there is a server error.
    /// </returns>
    [HttpPut("update")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [RoleAuthorization(Roles = "Admin")]
    public async Task<IActionResult> Update([FromBody] UserAccountDto dto)
    {
        var result = await userCommandService.Update(dto);

        return result.Match<IActionResult>(
            success: () => Ok(new { message = result.Info.Message }),
            failure: error => BadRequest(new { message = Error.Alert, errors = error.Message })
        );
    }

    /// <summary>
    /// Updates the username of an existing user.
    /// </summary>
    /// <param name="id">The unique identifier of the user to provide.</param>
    /// <param name="username">The new username to update.</param>
    /// <response code="200">The username was successfully updated.</response>
    /// <response code="400">The request was invalid or contained errors (e.g., validation failure).</response>
    /// <response code="500">An unexpected server error occurred while processing the request.</response>
    /// <returns>
    /// An <see cref="ActionResult"/> containing:
    /// - A 200 status code if the username update operation succeeds.
    /// - A 400 status code if the request is invalid, such as missing required fields or providing invalid data.
    /// - A 500 status code if a server-side error occurs, indicating the server was unable to process the request.
    /// </returns>
    [HttpPut("{id:int}/update-username")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [RoleAuthorization(Roles = "User")]
    public async Task<IActionResult> UpdateUsername([FromBody] int id, string username)
    {
        var result = await userCommandService.UpdateUserName(id, username);

        return result.Match<IActionResult>(
            success: () => Ok(new { message = result.Info.Message }),
            failure: error => BadRequest(new { message = Error.Alert, errors = error.Message })
        );
    }

    /// <summary>
    /// Updates the Email of an existing user.
    /// </summary>
    /// <param name="id">The unique identifier of the user to provide.</param>
    /// <param name="email">The new email to update.</param>
    /// <response code="200">The email was successfully updated.</response>
    /// <response code="400">The request was invalid or contained errors (e.g., validation failure).</response>
    /// <response code="500">An unexpected server error occurred while processing the request.</response>
    /// <returns>
    /// An <see cref="ActionResult"/> containing:
    /// - A 200 status code if the email update operation succeeds.
    /// - A 400 status code if the request is invalid, such as missing required fields or providing invalid data.
    /// - A 500 status code if a server-side error occurs, indicating the server was unable to process the request.
    /// </returns>
    [HttpPut("{id:int}/update-email")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [RoleAuthorization(Roles = "User")]
    public async Task<IActionResult> UpdateEmail(string email)
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

        var result = await loginCommandService.UpdateEmail(userId, email);

        return result.Match<IActionResult>(
            success: () => Ok(new { message = result.Info.Message }),
            failure: error => BadRequest(new { message = Error.Alert, errors = error.Message })
        );
    }

    /// <summary>
    /// Enables or disables a user account based on the provided data.
    /// </summary>
    /// <param name="dto">An object containing the user ID and the desired account status (enabled or disabled).</param>
    /// <response code="200">The user account status was successfully updated.</response>
    /// <response code="400">The request was invalid or contained errors (e.g., validation failure).</response>
    /// <response code="500">An unexpected server error occurred while processing the request.</response>
    /// <returns>
    /// An <see cref="ActionResult"/> containing:
    /// - A 200 status code if the operation succeeds and the account status is updated.
    /// - A 400 status code if the request is invalid, such as missing required fields or invalid data.
    /// - A 500 status code if an unexpected server-side error occurs, indicating the server was unable to process the request.
    /// </returns>
    [Authorize]
    [HttpPut("account-activation")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [RoleAuthorization(Roles = "Admin")]
    public async Task<IActionResult> AccountActivation([FromBody] AccountActivationDto dto)
    {
        var result = await userCommandService.ActivationAsync(dto);

        return result.Match<IActionResult>(
            success: () => Ok(new { message = result.Info.Message }),
            failure: error => BadRequest(new { message = Error.Alert, errors = error.Message })
        );
    }

    /// <summary>
    /// Deletes a user account and associated data.
    /// </summary>
    /// <param name="id">The unique identifier of the user to be deleted, provided by the client.</param>
    /// <response code="200">The user account was successfully deleted.</response>
    /// <response code="400">The request was invalid or contained errors.</response>
    /// <response code="404">The specified user was not found.</response>
    /// <response code="500">An unexpected server error occurred while processing the request.</response>
    /// <returns>
    /// An <see cref="ActionResult"/> containing:
    /// - A 200 status code with a confirmation message if the deletion succeeds.
    /// - A 400 status code if the request is invalid (e.g., missing or malformed ID).
    /// - A 404 status code if the user does not exist.
    /// - A 500 status code if an unexpected server-side error occurs.
    /// </returns>
    [HttpDelete("{id:int}/delete")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [RoleAuthorization(Roles = "User, Admin")]
    public async Task<IActionResult> Delete(int id)
    {
        var result = await userCommandService.Delete(id);

        return result.Match<IActionResult>(
            success: () => Ok(new { message = result.Info.Message }),
            failure: error => BadRequest(new { message = Error.Alert, errors = error.Message })
        );
    }
}
