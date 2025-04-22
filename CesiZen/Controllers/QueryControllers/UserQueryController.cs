using CesiZen.Application.Authorization;
using CesiZen.Domain.BusinessResult;
using CesiZen.Domain.DataTransfertObject;
using CesiZen.Domain.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace CesiZen.Api.Controllers;

[ApiController]
[Route("api/users/query")]
public class UserQueryController : ControllerBase
{
    private readonly IUserQueryService queryService;

    public UserQueryController(
        IUserQueryService queryService)
    {
        this.queryService = queryService;
    }

    /// <summary>
    /// Retrieves a paginated list of users filtered by a search term.
    /// </summary>
    /// <param name="pageNumber">The page number to retrieve, starting from 1.</param>
    /// <param name="pageSize">The number of users to include per page.</param>
    /// <param name="searchTerm">The keyword or term provided by the client to filter users.</param>
    /// <response code="200">The paginated list of users was successfully retrieved.</response>
    /// <response code="404">No users were found matching the search term.</response>
    /// <response code="500">An internal server error occurred while processing the request.</response>
    /// <returns>
    /// A list of user filtered by a search term.
    /// - A 200 status code with the paginated list of users matching the search term.
    /// - A 404 status code if no users are found for the given search term.
    /// - A 500 status code if there is a server error.
    /// </returns>
    [HttpGet("search")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [RoleAuthorization(Roles = "Admin")]
    public async Task<ActionResult<PagedResultDto<UserMinimumDto>>> SearchUsers([FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 10, [FromQuery] string searchTerm = "")
    {
        var parameters = new PageParametersDto()
        {
            PageNumber = pageNumber,
            PageSize = pageSize,
        };

        var result = await queryService.SearchUsers(parameters, searchTerm);

        return result.Match<ActionResult, PagedResultDto<UserMinimumDto>>(
             success: value => Ok(new { value }),
             failure: error => NotFound(new { message = Error.Alert, errors = error.Message })
        );
    }

    /// <summary>
    /// Retrieves a paginated list of users.
    /// </summary>
    /// <param name="pageNumber">The page number to retrieve, starting from 1.</param>
    /// <param name="pageSize">The number of users to include per page.</param>
    /// <response code="200">The paginated list of users was successfully retrieved.</response>
    /// <response code="404">No users were found for the specified page.</response>
    /// <response code="500">An internal server error occurred while processing the request.</response>
    /// <returns>
    /// A list of users
    /// - A 200 status code with the paginated list of users if successful.
    /// - A 404 status code if no users are found for the specified page.
    /// - A 500 status code if there is a server error.
    /// </returns>
    [HttpGet("index")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [RoleAuthorization(Roles = "Admin")]
    public async Task<ActionResult<PagedResultDto<UserMinimumDto>>> GetAllAsync([FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 10)
    {
        var result = await queryService.GetAllAsync(pageNumber, pageSize);

        return result.Match<ActionResult, PagedResultDto<UserMinimumDto>>(
             success: value => Ok(new { value }),
             failure: error => NotFound(new { message = Error.Alert, errors = error.Message })
        );
    }

    /// <summary>
    /// Retrieves a user by their unique identifier.
    /// </summary>
    /// <param name="id">The unique identifier of the user to provide.</param>
    /// <response code="200">The user was successfully retrieved.</response>
    /// <response code="404">No user was found for the specified ID.</response>
    /// <response code="500">An internal server error occurred while processing the request.</response>
    /// <returns>
    /// Desired user.
    /// - A 200 status code with the user data if found.
    /// - A 404 status code if the user is not found.
    /// - A 500 status code if there is a server error.
    /// </returns>
    [HttpGet("{id:int}/details")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [RoleAuthorization(Roles = "User, Admin")]
    public async Task<ActionResult<UserMinimumDto>> GetById(int id)
    {
        var result = await queryService.GetByIdAsync(id);
        return result.Match<ActionResult, UserMinimumDto>(
            success: value => Ok(new { value }),
            failure: error => BadRequest(new { message = Error.Alert, errors = error.Message })
        );
    }

    /// <summary>
    /// Retrieves a user by their unique username.
    /// </summary>
    /// <param name="username">The username of the user to provide.</param>
    /// <response code="200">The user was successfully retrieved.</response>
    /// <response code="404">No user was found for the specified Username.</response>
    /// <response code="500">An internal server error occurred while processing the request.</response>
    /// <returns>
    /// Desired user.
    /// - A 200 status code with the user data if found.
    /// - A 404 status code if the user is not found.
    /// - A 500 status code if there is a server error.
    /// </returns>
    [HttpGet("details")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [RoleAuthorization(Roles = "Admin")]
    public async Task<ActionResult<UserMinimumDto>> GetByName([FromQuery] string username)
    {
        var result = await queryService.GetByUsername(username);

        return result.Match<ActionResult, UserMinimumDto>(
            success: value => Ok(new { value }),
            failure: error => NotFound(new { message = Error.Alert, errors = error.Message })
        );
    }

    /// <summary>
    /// Retrieves a user profil by their unique identifier.
    /// </summary>
    /// <response code="200">The user was successfully retrieved.</response>
    /// <response code="404">No user was found for the specified ID.</response>
    /// <response code="500">An internal server error occurred while processing the request.</response>
    /// <returns>
    /// Desired user.
    /// - A 200 status code with the user data if found.
    /// - A 404 status code if the user is not found.
    /// - A 500 status code if there is a server error.
    /// </returns>
    [HttpGet("profile", Name = "GetProfile")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [RoleAuthorization(Roles = "User, Admin")]
    public async Task<ActionResult<UserDto>> GetProfile()
    {
        var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

        if (string.IsNullOrEmpty(userIdClaim))
        {
            return Unauthorized(new { message = Error.Alert, errors = UserErrors.NotConnected });
        }

        if (!int.TryParse(userIdClaim, out var userId))
        {
            return BadRequest(new { message = Error.Alert, errors = UserErrors.Unknown });
        }

        var result = await queryService.GetByIdAsync(userId);
        return result.Match<ActionResult, UserDto>(
            success: value => Ok(value),
            failure: error => BadRequest(new { message = Error.Alert, errors = error.Message })
        );
    }
}
