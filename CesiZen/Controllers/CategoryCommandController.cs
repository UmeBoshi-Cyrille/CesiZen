using CesiZen.Domain.BusinessResult;
using CesiZen.Domain.DataTransfertObject;
using CesiZen.Domain.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace CesiZen.Api.Controllers;

[ApiController]
[Route("/api/[controller]")]
public class CategoryCommandController : ControllerBase
{
    private readonly ICategoryCommandService categoryService;

    public CategoryCommandController(
        ICategoryCommandService categoryService)
    {
        this.categoryService = categoryService;
    }

    /// <summary>
    /// Create new category
    /// </summary>
    /// <param name="dto">data provided by the client</param>
    /// <response code="200">operation succeeded</response>
    /// <response code="400">Bad request</response>
    /// <response code="500">service unvalaible</response>
    /// <returns></returns>
    [HttpPost("create")]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> Create([FromBody] CategoryDto dto)
    {
        var result = await categoryService.Insert(dto);

        return result.Match<ActionResult>(
            success: () => CreatedAtAction(
                nameof(CategoryQueryController.GetCategory),
                nameof(CategoryQueryController),
                new { name = dto.Name, message = result.Info.Message }, dto),
            failure: error => BadRequest(new { message = error.Message })
        );
    }

    /// <summary>
    /// Update category
    /// </summary>
    /// <param name="dto">data provided by the client</param>
    /// <response code="200">operation succeeded</response>
    /// <response code="400">Bad request</response>
    /// <response code="500">service unvalaible</response>
    /// <returns></returns>
    [HttpPut("update/{id}")]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> Update([FromBody] CategoryDto dto)
    {
        var result = await categoryService.Update(dto);

        return result.Match<IActionResult>(
            success: () => Ok(new { result.Info.Message }),
            failure: error => BadRequest(new { message = error.Message })
        );
    }

    /// <summary>
    /// Delete category
    /// </summary>
    /// <param name="id">id provided by the client</param>
    /// <response code="200">operation succeeded</response>
    /// <response code="400">Bad request</response>
    /// <response code="500">service unvalaible</response>
    /// <returns></returns>
    [HttpDelete("delete/{id}")]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> Delete(string id)
    {
        var result = await categoryService.Delete(id);

        return result.Match<IActionResult>(
            success: () => Ok(new { result.Info.Message }),
            failure: error => BadRequest(new { message = error.Message })
        );
    }
}
