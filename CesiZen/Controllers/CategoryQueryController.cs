using CesiZen.Domain.BusinessResult;
using CesiZen.Domain.DataTransfertObject;
using CesiZen.Domain.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace CesiZen.Api.Controllers;

[ApiController]
[Route("/api/[controller]")]
public class CategoryQueryController : ControllerBase
{
    private readonly ICategoryQueryService categoryService;

    public CategoryQueryController(ICategoryQueryService categoryService)
    {
        this.categoryService = categoryService;
    }

    /// <summary>
    /// Get paginated categories
    /// </summary>
    /// <param name="pageNumber">last record</param>
    /// <param name="pageSize">number of element by page</param>
    /// <response code="200">data retrieved</response>
    /// <response code="404">Not Found</response>
    /// <response code="500">service unvalaible</response>
    /// <returns></returns>
    [HttpGet("categories")]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<PagedResult<CategoryRequestDto>>> GetCategories([FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 10)
    {
        var result = await categoryService.GetAllAsync(pageNumber, pageSize);

        return result.Match<ActionResult, PagedResult<CategoryRequestDto>>(
             success: value => Ok(new { value }),
             failure: error => NotFound(new { message = error.Message })
        );
    }

    /// <summary>
    /// Get category by id
    /// </summary>
    /// <param name="id">id provided by the client</param>
    /// <response code="200">data retrieved</response>
    /// <response code="404">Not Found</response>
    /// <response code="500">service unvalaible</response>
    /// <returns></returns>
    [HttpGet("category/{id}")]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<CategoryRequestDto>> GetCategory(int Id)
    {
        var result = await categoryService.GetByIdAsync(id);
        return result.Match<ActionResult, CategoryRequestDto>(
            success: value => Ok(new { value }),
            failure: error => NotFound(new { message = error.Message })
        );
    }
}
