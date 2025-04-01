using CesiZen.Application.Authorization;
using CesiZen.Domain.BusinessResult;
using CesiZen.Domain.DataTransfertObject;
using CesiZen.Domain.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace CesiZen.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ArticleCommandController : ControllerBase
{
    private readonly IArticleCommandService articleCommandService;

    public ArticleCommandController(
        IArticleCommandService articleCommandService)
    {
        this.articleCommandService = articleCommandService;
    }

    /// <summary>
    /// Create new article
    /// </summary>
    /// <param name="dto">data provided by the client</param>
    /// <response code="201">ObjectCreated</response>
    /// <response code="400">Bad request</response>
    /// <response code="500">service unvalaible</response>
    /// <returns></returns>
    [HttpPost("create")]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [RoleAuthorization(Roles = "Admin")]
    public async Task<IActionResult> Create([FromBody] NewArticleDto dto)
    {
        var result = await articleCommandService.Insert(dto);

        return result.Match<ArticleMinimumDto, ActionResult>(
            success: createdArticle => CreatedAtAction(
                nameof(ArticleQueryController.GetArticle),
                "ArticleQueryController",
                new { id = createdArticle.Id },
                new { message = "result.Info.Message", article = createdArticle }),
            failure: error => BadRequest(new { message = error.Message })
        );
    }

    /// <summary>
    /// Update article data
    /// </summary>
    /// <param name="id">id provided by the client</param>
    /// <param name="dto">data provided by the client</param>
    /// <response code="200">operation succeeded</response>
    /// <response code="400">Bad request</response>
    /// <response code="500">service unvalaible</response>
    /// <returns></returns>
    [HttpPut("update/{id}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [RoleAuthorization(Roles = "Admin")]
    public async Task<IActionResult> Update(int id, [FromBody] ArticleDto dto)
    {
        dto.Id = id;
        var result = await articleCommandService.Update(dto);

        return result.Match<IActionResult>(
            success: () => Ok(new { message = result.Info.Message }),
            failure: error => BadRequest(new { message = error.Message })
        );
    }

    /// <summary>
    /// Update article title
    /// </summary>
    /// <param name="id">id provided by the client</param>
    /// <param name="title">title provided by the client</param>
    /// <response code="200">operation succeeded</response>
    /// <response code="400">Bad request</response>
    /// <response code="500">service unvalaible</response>
    /// <returns></returns>
    [HttpPut("update-title/{id}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [RoleAuthorization(Roles = "Admin")]
    public async Task<IActionResult> UpdateTitle(int id, [FromBody] string title)
    {
        var result = await articleCommandService.UpdateTitleAsync(id, title);

        return result.Match<IActionResult>(
            success: () => Ok(new { message = result.Info.Message }),
            failure: error => BadRequest(new { message = error.Message })
        );
    }

    /// <summary>
    /// Update article description
    /// </summary>
    /// <param name="id">id provided by the client</param>
    /// <param name="description">description provided by the client</param>
    /// <response code="200">operation succeeded</response>
    /// <response code="400">Bad request</response>
    /// <response code="500">service unvalaible</response>
    /// <returns></returns>
    [HttpPut("update-description/{id}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [RoleAuthorization(Roles = "Admin")]
    public async Task<IActionResult> UpdateDescription(int id, [FromBody] string description)
    {
        var result = await articleCommandService.UpdateDescriptionAsync(id, description);

        return result.Match<IActionResult>(
            success: () => Ok(new { message = result.Info.Message }),
            failure: error => BadRequest(new { message = error.Message })
        );
    }

    /// <summary>
    /// Update article content
    /// </summary>
    /// <param name="id">id provided by the client</param>
    /// <param name="content">content provided by the client</param>
    /// <response code="200">operation succeeded</response>
    /// <response code="400">Bad request</response>
    /// <response code="500">service unvalaible</response>
    /// <returns></returns>
    [HttpPut("update-content/{id}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [RoleAuthorization(Roles = "Admin")]
    public async Task<IActionResult> UpdateContent(int id, [FromBody] string content)
    {
        var result = await articleCommandService.UpdateContentAsync(id, content);

        return result.Match<IActionResult>(
            success: () => Ok(new { message = result.Info.Message }),
            failure: error => BadRequest(new { message = error.Message })
        );
    }

    /// <summary>
    /// Delete article
    /// </summary>
    /// <param name="id">id provided by the client</param>
    /// <response code="200">operation succeeded</response>
    /// <response code="400">Bad request</response>
    /// <response code="500">service unvalaible</response>
    /// <returns></returns>
    [HttpDelete("delete/{id}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [RoleAuthorization(Roles = "Admin")]
    public async Task<IActionResult> Delete(int id)
    {
        var result = await articleCommandService.Delete(id);

        return result.Match<IActionResult>(
            success: () => Ok(new { message = result.Info.Message }),
            failure: error => BadRequest(new { message = error.Message })
        );
    }
}
