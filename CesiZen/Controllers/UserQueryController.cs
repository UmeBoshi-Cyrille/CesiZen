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

    [HttpGet("search-users")]
    public async Task<ActionResult<PagedResult<UserRequestDto>>> SearchUsers([FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 10, [FromQuery] string searchTerm = null)
    {
        var parameters = new PageParameters()
        {
            PageNumber = pageNumber,
            PageSize = pageSize,
        };

        var result = await queryService.SearchUsers(parameters, searchTerm);

        return result.Match<ActionResult, PagedResult<UserRequestDto>>(
             success: value => Ok(new { value }),
             failure: error => BadRequest(new { message = error.Message })
        );
    }

    [HttpGet("users")]
    public async Task<ActionResult<PagedResult<UserRequestDto>>> GetAllAsync([FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 10)
    {
        var result = await queryService.GetAllAsync(pageNumber, pageSize);

        return result.Match<ActionResult, PagedResult<UserRequestDto>>(
             success: value => Ok(new { value }),
             failure: error => BadRequest(new { message = error.Message })
        );
    }

    [HttpGet("user/{id}")]
    public async Task<ActionResult<UserRequestDto>> GetById(string id)
    {
        var result = await queryService.GetByIdAsync(id);
        return result.Match<ActionResult, UserRequestDto>(
            success: value => Ok(new { value }),
            failure: error => BadRequest(new { message = error.Message })
        );
    }

    [HttpGet("user")]
    public async Task<ActionResult<UserRequestDto>> GetByName([FromQuery] string username)
    {
        var result = await queryService.GetByUsername(username);

        return result.Match<ActionResult, UserRequestDto>(
            success: value => Ok(new { value }),
            failure: error => BadRequest(new { message = error.Message })
        );
    }
}
