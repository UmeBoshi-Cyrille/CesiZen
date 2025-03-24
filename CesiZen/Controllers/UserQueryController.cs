using CesiZen.Application.Authorization;
using CesiZen.Domain.BusinessResult;
using CesiZen.Domain.DataTransfertObject;
using CesiZen.Domain.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace CesiZen.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class UserQueryController : ControllerBase
{
    private readonly IUserQueryService queryService;

    public UserQueryController(
        IUserQueryService queryService)
    {
        this.queryService = queryService;
    }

    /// <summary>
    /// Get paginated users by term.
    /// </summary>
    /// <param name="pageNumber">last record</param>
    /// <param name="pageSize">number of element by page</param>
    /// <param name="searchTerm">term provided by the client for the research</param>
    /// <response code="200">data retrieved</response>
    /// <response code="404">Not Found</response>
    /// <response code="500">service unvalaible</response>
    /// <returns></returns>
    [HttpGet("search-users")]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [RoleAuthorization(Roles = "Admin")]
    public async Task<ActionResult<PagedResultDto<UserRequestDto>>> SearchUsers([FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 10, [FromQuery] string searchTerm = "")
    {
        var parameters = new PageParametersDto()
        {
            PageNumber = pageNumber,
            PageSize = pageSize,
        };

        var result = await queryService.SearchUsers(parameters, searchTerm);

        return result.Match<ActionResult, PagedResultDto<UserRequestDto>>(
             success: value => Ok(new { value }),
             failure: error => NotFound(new { message = error.Message })
        );
    }

    /// <summary>
    /// Get paginated users
    /// </summary>
    /// <param name="pageNumber">last record</param>
    /// <param name="pageSize">number of element by page</param>
    /// <response code="200">data retrieved</response>
    /// <response code="404">Not Found</response>
    /// <response code="500">service unvalaible</response>
    /// <returns></returns>
    [HttpGet("users")]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [RoleAuthorization(Roles = "Admin")]
    public async Task<ActionResult<PagedResultDto<UserRequestDto>>> GetAllAsync([FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 10)
    {
        var result = await queryService.GetAllAsync(pageNumber, pageSize);

        return result.Match<ActionResult, PagedResultDto<UserRequestDto>>(
             success: value => Ok(new { value }),
             failure: error => NotFound(new { message = error.Message })
        );
    }

    /// <summary>
    /// Get user by id
    /// </summary>
    /// <param name="id">id provided by the client</param>
    /// <response code="200">data retrieved</response>
    /// <response code="404">Not Found</response>
    /// <response code="500">service unvalaible</response>
    /// <returns></returns>
    [HttpGet("user/{id}")]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [RoleAuthorization(Roles = "User")]
    public async Task<ActionResult<UserRequestDto>> GetById(int id)
    {
        var result = await queryService.GetByIdAsync(id);
        return result.Match<ActionResult, UserRequestDto>(
            success: value => Ok(new { value }),
            failure: error => BadRequest(new { message = error.Message })
        );
    }

    /// <summary>
    /// Get user by user name
    /// </summary>
    /// <param name="username">username provided by the client</param>
    /// <response code="200">data retrieved</response>
    /// <response code="404">Not Found</response>
    /// <response code="500">service unvalaible</response>
    /// <returns></returns>
    [HttpGet("user")]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [RoleAuthorization(Roles = "Admin")]
    public async Task<ActionResult<UserRequestDto>> GetByName([FromQuery] string username)
    {
        var result = await queryService.GetByUsername(username);

        return result.Match<ActionResult, UserRequestDto>(
            success: value => Ok(new { value }),
            failure: error => NotFound(new { message = error.Message })
        );
    }
}
