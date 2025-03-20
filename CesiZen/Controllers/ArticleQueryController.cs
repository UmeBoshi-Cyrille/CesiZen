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

    /// <summary>
    /// Get paginated articles by term.
    /// </summary>
    /// <param name="pageNumber">last record</param>
    /// <param name="pageSize">number of element by page</param>
    /// <param name="searchTerm">term provided by the client for the research</param>
    /// <response code="200">data retrieved</response>
    /// <response code="404">Not Found</response>
    /// <response code="500">service unvalaible</response>
    /// <returns></returns>
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

    /// <summary>
    /// Get paginated articles
    /// </summary>
    /// <param name="pageNumber">last record</param>
    /// <param name="pageSize">number of element by page</param>
    /// <response code="200">data retrieved</response>
    /// <response code="404">Not Found</response>
    /// <response code="500">service unvalaible</response>
    /// <returns></returns>
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

    /// <summary>
    /// Get article by id
    /// </summary>
    /// <param name="id">id provided by the client</param>
    /// <response code="200">data retrieved</response>
    /// <response code="404">Not Found</response>
    /// <response code="500">service unvalaible</response>
    /// <returns></returns>
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
