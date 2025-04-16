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
    /// Creates a new category resource.
    /// </summary>
    /// <param name="name">The name of the new category to create.</param>
    /// <response code="201">The category was successfully created.</response>
    /// <response code="400">The request was invalid or contained errors (e.g., validation failure).</response>
    /// <response code="500">An unexpected server error occurred while processing the request.</response>
    /// <returns>
    /// An <see cref="ActionResult"/> containing:
    /// - A 201 status code with the details of the newly created category if the operation succeeds.
    /// - A 400 status code if the request is invalid, such as missing required fields or failing validation checks.
    /// - A 500 status code if an unexpected server-side error occurs during the creation process.
    /// </returns>
    [HttpPost("create")]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [RoleAuthorization(Roles = "Admin")]
    public async Task<IActionResult> Create([FromBody] string name)
    {
        var dto = new CategoryDto { Name = name };
        var result = await categoryService.Insert(dto);

        return result.Match<CategoryDto, ActionResult>(
            success: createdCategory => CreatedAtAction(
                nameof(CategoryQueryController.GetCategory),
                "CategoryQuery",
                new { id = createdCategory.Id },
                new { data = createdCategory, message = result.Info.Message }),
            failure: error => BadRequest(new { message = error.Message })
        );
    }

    /// <summary>
    /// Updates the details of an existing category.
    /// </summary>
    /// <param name="dto">An object to provide containing the updated data for the category.</param>
    /// <response code="200">The category was successfully updated.</response>
    /// <response code="400">The request was invalid or contained errors (e.g., validation failure).</response>
    /// <response code="500">An unexpected server error occurred while processing the request.</response>
    /// <returns>
    /// An <see cref="ActionResult"/> containing:
    /// - A 200 status code if the update operation succeeds and the category details are modified successfully.
    /// - A 400 status code if the request is invalid, such as missing required fields or providing invalid data.
    /// - A 500 status code if an unexpected server-side error occurs during the update process.
    /// </returns>
    [HttpPut("{id:int}/update")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [RoleAuthorization(Roles = "Admin")]
    public async Task<IActionResult> Update(int id, [FromBody] CategoryDto dto)
    {
        dto.Id = id;
        var result = await categoryService.Update(dto);

        return result.Match<IActionResult>(
            success: () => Ok(new { result.Info.Message }),
            failure: error => BadRequest(new { message = error.Message })
        );
    }

    /// <summary>
    /// Deletes a category resource identified by its unique ID.
    /// </summary>
    /// <param name="id">The unique identifier of the category to be deleted, to provide.</param>
    /// <response code="204">The category was successfully deleted.</response>
    /// <response code="404">The specified category was not found.</response>
    /// <response code="400">The request was invalid or contained errors.</response>
    /// <response code="500">An unexpected server error occurred while processing the request.</response>
    /// <returns>
    /// An <see cref="ActionResult"/> containing:
    /// - A 204 status code if the category is successfully deleted and no content is returned.
    /// - A 404 status code if the category does not exist.
    /// - A 400 status code if the request is invalid (e.g., malformed ID).
    /// - A 500 status code if an unexpected server-side error occurs during the deletion process.
    /// </returns>
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
