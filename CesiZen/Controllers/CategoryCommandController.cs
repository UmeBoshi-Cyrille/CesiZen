using CesiZen.Application.Authorization;
using CesiZen.Domain.BusinessResult;
using CesiZen.Domain.DataTransfertObject;
using CesiZen.Domain.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace CesiZen.Api.Controllers;

[ApiController]
[Route("/api/categories/command")]
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
    /// <response code="201">operation succeeded</response>
    /// <response code="400">Bad request</response>
    /// <response code="500">service unvalaible</response>
    /// <returns></returns>
    [HttpPost("create")]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [RoleAuthorization(Roles = "Admin")]
    public async Task<IActionResult> Create([FromBody] CategoryDto dto)
    {
        var result = await categoryService.Insert(dto);

        return result.Match<CategoryDto, ActionResult>(
            success: createdCategory => CreatedAtAction(
                nameof(CategoryQueryController.GetCategory),
                "CategoryQueryController",
                new { id = createdCategory.Id },
                new { message = result.Info.Message, category = createdCategory }),
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
    [HttpPut("{id:int}/update")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [RoleAuthorization(Roles = "Admin")]
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
    [HttpDelete("{id:int}/delete")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [RoleAuthorization(Roles = "Admin")]
    public async Task<IActionResult> Delete(int id)
    {
        var result = await categoryService.Delete(id);

        return result.Match<IActionResult>(
            success: () => Ok(new { result.Info.Message }),
            failure: error => BadRequest(new { message = error.Message })
        );
    }
}
