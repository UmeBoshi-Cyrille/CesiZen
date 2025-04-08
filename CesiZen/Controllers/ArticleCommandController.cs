using CesiZen.Application.Authorization;
using CesiZen.Domain.BusinessResult;
using CesiZen.Domain.DataTransfertObject;
using CesiZen.Domain.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace CesiZen.Api.Controllers;

[ApiController]
[Route("api/articles/command")]
public class ArticleCommandController : ControllerBase
{
    private readonly IArticleCommandService articleCommandService;

    public ArticleCommandController(
        IArticleCommandService articleCommandService)
    {
        this.articleCommandService = articleCommandService;
    }

    /// <summary>
    /// Creates a new article resource.
    /// </summary>
    /// <param name="dto">An object to provide containing the data required to create the article.</param>
    /// <response code="201">The article was successfully created.</response>
    /// <response code="400">The request was invalid or contained errors (e.g., validation failure).</response>
    /// <response code="500">An unexpected server error occurred while processing the request.</response>
    /// <returns>
    /// An <see cref="ActionResult"/> containing:
    /// - A 201 status code with the details of the newly created article if the operation succeeds.
    /// - A 400 status code if the request is invalid, such as missing required fields or failing validation checks.
    /// - A 500 status code if an unexpected server-side error occurs during the creation process.
    /// </returns>
    [HttpPost("create")]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [RoleAuthorization(Roles = "Admin")]
    public async Task<IActionResult> Create([FromBody] NewArticleDto dto)
    {
        try
        {
            if (!User.Identity?.IsAuthenticated ?? false)
            {
                return Unauthorized(new { message = "not authenticated" });
            }

            if (!User.IsInRole("Admin"))
            {
                return Forbid();
            }

            var result = await articleCommandService.Insert(dto);

            return result.Match<ArticleMinimumDto, ActionResult>(
            success: createdArticle => CreatedAtAction(
                nameof(ArticleQueryController.GetArticle),
                "ArticleQuery",
                new { id = createdArticle.Id },
                new { data = createdArticle, message = result.Info.Message }),
            failure: error => BadRequest(new { message = error.Message })
        );
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.ToString());
        }

        return BadRequest();
    }

    /// <summary>
    /// Updates the data of an existing article identified by its unique ID.
    /// </summary>
    /// <param name="id">The unique identifier to provide of the article to be updated.</param>
    /// <param name="dto">An object to provide containing the updated data for the article.</param>
    /// <response code="200">The article was successfully updated.</response>
    /// <response code="400">The request was invalid or contained errors (e.g., validation failure).</response>
    /// <response code="500">An unexpected server error occurred while processing the request.</response>
    /// <returns>
    /// An <see cref="ActionResult"/> containing:
    /// - A 200 status code if the update operation succeeds and the article is modified successfully.
    /// - A 400 status code if the request is invalid (e.g., missing required fields or failing validation checks).
    /// - A 500 status code if an unexpected server-side error occurs during processing.
    /// </returns>
    [HttpPut("{id:int}/update")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [RoleAuthorization(Roles = "Admin")]
    public async Task<IActionResult> Update(int id, [FromBody] ArticleDto dto)
    {
        if (!User.Identity?.IsAuthenticated ?? false)
        {
            return Unauthorized(new { message = "not authenticated" });
        }

        if (!User.IsInRole("Admin"))
        {
            return Forbid();
        }

        dto.Id = id;
        var result = await articleCommandService.Update(dto);

        return result.Match<IActionResult>(
            success: () => Ok(new { message = result.Info.Message }),
            failure: error => BadRequest(new { message = error.Message })
        );
    }

    /// <summary>
    /// Updates the title of an existing article identified by its unique ID.
    /// </summary>
    /// <param name="id">The unique identifier to provide of the article to update.</param>
    /// <param name="title">The new title for the article.</param>
    /// <response code="200">The article title was successfully updated.</response>
    /// <response code="400">The request was invalid or contained errors (e.g., validation failure).</response>
    /// <response code="500">An unexpected server error occurred while processing the request.</response>
    /// <returns>
    /// An <see cref="ActionResult"/> containing:
    /// - A 200 status code if the title is successfully updated.
    /// - A 400 status code if the request is invalid (e.g., missing or malformed title).
    /// - A 500 status code if an unexpected server-side error occurs during processing.
    /// </returns>
    [HttpPut("{id:int}/update-title")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [RoleAuthorization(Roles = "Admin")]
    public async Task<IActionResult> UpdateTitle(int id, [FromBody] string title)
    {
        if (!User.Identity?.IsAuthenticated ?? false)
        {
            return Unauthorized(new { message = "not authenticated" });
        }

        if (!User.IsInRole("Admin"))
        {
            return Forbid();
        }

        var result = await articleCommandService.UpdateTitleAsync(id, title);

        return result.Match<IActionResult>(
            success: () => Ok(new { message = result.Info.Message }),
            failure: error => BadRequest(new { message = error.Message })
        );
    }

    /// <summary>
    /// Updates the description of an existing article identified by its unique ID.
    /// </summary>
    /// <param name="id">The unique identifier of the article to update.</param>
    /// <param name="description">The new description for the article.</param>
    /// <response code="200">The article description was successfully updated.</response>
    /// <response code="400">The request was invalid or contained errors (e.g., validation failure).</response>
    /// <response code="500">An unexpected server error occurred while processing the request.</response>
    /// <returns>
    /// An <see cref="ActionResult"/> containing:
    /// - A 200 status code if the description is successfully updated.
    /// - A 400 status code if the request is invalid (e.g., missing or malformed description).
    /// - A 500 status code if an unexpected server-side error occurs during processing.
    /// </returns>
    [HttpPut("{id:int}/update-description")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [RoleAuthorization(Roles = "Admin")]
    public async Task<IActionResult> UpdateDescription(int id, [FromBody] string description)
    {
        if (!User.Identity?.IsAuthenticated ?? false)
        {
            return Unauthorized(new { message = "not authenticated" });
        }

        if (!User.IsInRole("Admin"))
        {
            return Forbid();
        }

        var result = await articleCommandService.UpdateDescriptionAsync(id, description);

        return result.Match<IActionResult>(
            success: () => Ok(new { message = result.Info.Message }),
            failure: error => BadRequest(new { message = error.Message })
        );
    }

    /// <summary>
    /// Updates the content of an existing article identified by its unique ID.
    /// </summary>
    /// <param name="id">The unique identifier of the article to update.</param>
    /// <param name="content">The new content for the article.</param>
    /// <response code="200">The article content was successfully updated.</response>
    /// <response code="400">The request was invalid or contained errors (e.g., validation failure).</response>
    /// <response code="500">An unexpected server error occurred while processing the request.</response>
    /// <returns>
    /// An <see cref="ActionResult"/> containing:
    /// - A 200 status code if the content is successfully updated.
    /// - A 400 status code if the request is invalid (e.g., missing or malformed content).
    /// - A 500 status code if an unexpected server-side error occurs during processing.
    /// </returns>
    [HttpPut("{id:int}/update-content")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [RoleAuthorization(Roles = "Admin")]
    public async Task<IActionResult> UpdateContent(int id, [FromBody] string content)
    {
        if (!User.Identity?.IsAuthenticated ?? false)
        {
            return Unauthorized(new { message = "not authenticated" });
        }

        if (!User.IsInRole("Admin"))
        {
            return Forbid();
        }

        var result = await articleCommandService.UpdateContentAsync(id, content);

        return result.Match<IActionResult>(
            success: () => Ok(new { message = result.Info.Message }),
            failure: error => BadRequest(new { message = error.Message })
        );
    }

    /// <summary>
    /// Deletes an article resource identified by its unique ID.
    /// </summary>
    /// <param name="id">The unique identifier of the article to be deleted.</param>
    /// <response code="204">The article was successfully deleted.</response>
    /// <response code="400">The request was invalid or contained errors.</response>
    /// <response code="500">An unexpected server error occurred while processing the request.</response>
    /// <returns>
    /// An <see cref="ActionResult"/> containing:
    /// - A 204 status code if the article is successfully deleted and no content is returned.
    /// - A 400 status code if the request is invalid (e.g., malformed or missing ID).
    /// - A 500 status code if an unexpected server-side error occurs during processing.
    /// </returns>
    [HttpDelete("{id:int}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [RoleAuthorization(Roles = "Admin")]
    public async Task<IActionResult> Delete(int id)
    {
        if (!User.Identity?.IsAuthenticated ?? false)
        {
            return Unauthorized(new { message = "not authenticated" });
        }

        if (!User.IsInRole("Admin"))
        {
            return Forbid();
        }

        var result = await articleCommandService.Delete(id);

        return result.Match<IActionResult>(
            success: () => Ok(new { message = result.Info.Message }),
            failure: error => BadRequest(new { message = error.Message })
        );
    }
}
