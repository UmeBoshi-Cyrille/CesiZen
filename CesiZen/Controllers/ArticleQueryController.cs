using CesiZen.Domain.BusinessResult;
using CesiZen.Domain.DataTransfertObject;
using CesiZen.Domain.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace CesiZen.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ArticleQueryController : ControllerBase
{
    private readonly IArticleQueryService articleService;

    public ArticleQueryController(IArticleQueryService articleService)
    {
        this.articleService = articleService;
    }

    [HttpGet("search-articles")]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<PagedResult<ArticleDto>>> SearchArticles(int pageNumber = 1, int pageSize = 10, [FromQuery] string searchTerm = "")
    {
        var parameters = new PageParameters()
        {
            PageNumber = pageNumber,
            PageSize = pageSize,
        };

        var result = await articleService.SearchArticles(parameters, searchTerm);

        return result.Match<ActionResult, PagedResult<ArticleDto>>(
             success: value => Ok(new { value }),
             failure: error => NotFound(new { message = error.Message })
        );
    }

    [HttpGet("articles")]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<PagedResult<ArticleDto>>> GetArticles(int pageNumber = 1, int pageSize = 10, [FromQuery] string searchTerm = "")
    {
        var result = await articleService.GetAllAsync(pageNumber, pageSize);

        return result.Match<ActionResult, PagedResult<ArticleDto>>(
             success: value => Ok(new { value }),
             failure: error => NotFound(new { message = error.Message })
        );
    }

    [HttpGet("article/{id}")]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<ArticleDto>> GetArticle(string id)
    {
        var result = await articleService.GetByIdAsync(id);
        return result.Match<ActionResult, ArticleDto>(
            success: value => Ok(new { value }),
            failure: error => NotFound(new { message = error.Message })
        );
    }
}
