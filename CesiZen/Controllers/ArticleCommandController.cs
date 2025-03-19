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

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] ArticleDto article)
    {
        var result = articleCommandService.Insert(article).Result;

        return result.Match<ActionResult>(
            success: () => CreatedAtAction(
                nameof(ArticleQueryController.GetArticle),
                nameof(ArticleQueryController),
                new { id = article.Id, article = article }),
            failure: error => BadRequest(error)
        );
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update([FromBody] ArticleDto article)
    {
        var result = await articleCommandService.Update(article);

        var message = "OK";
        return result.Match<IActionResult>(
            success: () => Ok(new { message }),
            failure: error => BadRequest(new { error })
        );
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateTitle(string id, [FromBody] string title)
    {
        var result = await articleCommandService.UpdateTitleAsync(id, title);

        var message = "OK";
        return result.Match<IActionResult>(
            success: () => Ok(new { message }),
            failure: error => BadRequest(new { error })
        );
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateDescription(string id, [FromBody] string description)
    {
        var result = await articleCommandService.UpdateDescriptionAsync(id, description);

        var message = "OK";
        return result.Match<IActionResult>(
            success: () => Ok(new { message }),
            failure: error => BadRequest(new { error })
        );
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateContent(string id, [FromBody] string content)
    {
        var result = await articleCommandService.UpdateContentAsync(id, content);

        var message = "OK";
        return result.Match<IActionResult>(
            success: () => Ok(new { message }),
            failure: error => BadRequest(new { error })
        );
    }


    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(string id)
    {
        var result = await articleCommandService.Delete(id);

        var message = "OK";
        return result.Match<IActionResult>(
            success: () => Ok(new { message }),
            failure: error => BadRequest(new { error })
        );
    }
}
