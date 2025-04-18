using CesiZen.Domain.BusinessResult;
using CesiZen.Domain.DataTransfertObject;
using CesiZen.Domain.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CesiZen.Api.Controllers;

[ApiController]
[Route("/api/categories/query")]
public class CategoryQueryController : ControllerBase
{
    private readonly ICategoryQueryService categoryService;

    public CategoryQueryController(ICategoryQueryService categoryService)
    {
        this.categoryService = categoryService;
    }

    /// <summary>
    /// Retrieves a paginated list of categories.
    /// </summary>
    /// <param name="pageNumber">The page number to retrieve, starting from 1.</param>
    /// <param name="pageSize">The number of categories to include per page.</param>
    /// <response code="200">The paginated list of categories was successfully retrieved.</response>
    /// <response code="404">No categories were found for the specified page.</response>
    /// <response code="500">An internal server error occurred while processing the request.</response>
    /// <returns>
    /// An <see cref="ActionResult"/> containing:
    /// - A 200 status code with the paginated list of categories if successful.
    /// - A 404 status code if no categories are found for the specified page.
    /// - A 500 status code if there is a server error.
    /// </returns>
    [HttpGet("index")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [AllowAnonymous]
    public async Task<ActionResult<PagedResultDto<CategoryResponseDto>>> GetCategories([FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 10)
    {
        var result = await categoryService.GetAllAsync(pageNumber, pageSize);

        return result.Match<ActionResult, PagedResultDto<CategoryResponseDto>>(
             success: value => Ok(new { value }),
             failure: error => NotFound(new { message = Error.Alert, errors = error.Message })
        );
    }

    /// <summary>
    /// Retrieves a category by its unique identifier.
    /// </summary>
    /// <param name="id">The unique identifier of the category to provide.</param>
    /// <response code="200">The category was successfully retrieved.</response>
    /// <response code="404">No category was found for the specified ID.</response>
    /// <response code="500">An internal server error occurred while processing the request.</response>
    /// <returns>
    /// the category corresponding to the unique identifier.
    /// - A 200 status code with the category data if found.
    /// - A 404 status code if the category is not found.
    /// - A 500 status code if there is a server error.
    /// </returns>
    [HttpGet("{id:int}/details")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [AllowAnonymous]
    public async Task<ActionResult<CategoryResponseDto>> GetCategory(int id)
    {
        var result = await categoryService.GetByIdAsync(id);
        return result.Match<ActionResult, CategoryResponseDto>(
            success: value => Ok(new { value }),
            failure: error => NotFound(new { message = Error.Alert, errors = error.Message })
        );
    }
}
