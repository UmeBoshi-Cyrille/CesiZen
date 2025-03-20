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

    [HttpPost("create")]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> Create([FromBody] ArticleDto article)
    {
        var result = await articleCommandService.Insert(article);

        return result.Match<ActionResult>(
            success: () => CreatedAtAction(
                nameof(ArticleQueryController.GetArticle),
                nameof(ArticleQueryController),
                new { id = article.Id, article = article, message = result.Info.Message }),
            failure: error => BadRequest(new { message = error.Message })
        );
    }

    [HttpPut("update/{id}")]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> Update([FromBody] ArticleDto article)
    {
        var result = await articleCommandService.Update(article);

        return result.Match<IActionResult>(
            success: () => Ok(new { message = result.Info.Message }),
            failure: error => BadRequest(new { message = error.Message })
        );
    }

    [HttpPut("update-title/{id}")]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> UpdateTitle(string id, [FromBody] string title)
    {
        var result = await articleCommandService.UpdateTitleAsync(id, title);

        return result.Match<IActionResult>(
            success: () => Ok(new { message = result.Info.Message }),
            failure: error => BadRequest(new { message = error.Message })
        );
    }

    [HttpPut("update-description/{id}")]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> UpdateDescription(string id, [FromBody] string description)
    {
        var result = await articleCommandService.UpdateDescriptionAsync(id, description);

        return result.Match<IActionResult>(
            success: () => Ok(new { message = result.Info.Message }),
            failure: error => BadRequest(new { message = error.Message })
        );
    }

    [HttpPut("update-content/{id}")]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> UpdateContent(string id, [FromBody] string content)
    {
        var result = await articleCommandService.UpdateContentAsync(id, content);

        return result.Match<IActionResult>(
            success: () => Ok(new { message = result.Info.Message }),
            failure: error => BadRequest(new { message = error.Message })
        );
    }


    [HttpDelete("delete/{id}")]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> Delete(string id)
    {
        var result = await articleCommandService.Delete(id);

        return result.Match<IActionResult>(
            success: () => Ok(new { message = result.Info.Message }),
            failure: error => BadRequest(new { message = error.Message })
        );
    }
}
