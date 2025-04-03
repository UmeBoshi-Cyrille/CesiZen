using CesiZen.Domain.BusinessResult;
using CesiZen.Domain.DataTransfertObject;
using CesiZen.Domain.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace CesiZen.Api.Controllers;

[ApiController]
[Route("api/articles/query")]
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
    [HttpGet("search")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<PagedResultDto<ArticleMinimumDto>>> SearchArticles(int pageNumber = 1, int pageSize = 10, [FromQuery] string searchTerm = "")
    {
        var parameters = new PageParametersDto()
        {
            PageNumber = pageNumber,
            PageSize = pageSize,
        };

        var result = await articleService.SearchArticles(parameters, searchTerm);

        return result.Match<ActionResult, PagedResultDto<ArticleMinimumDto>>(
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
    [HttpGet("get")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<PagedResultDto<ArticleMinimumDto>>> GetArticles(int pageNumber = 1, int pageSize = 10)
    {
        var result = await articleService.GetAllAsync(pageNumber, pageSize);

        return result.Match<ActionResult, PagedResultDto<ArticleMinimumDto>>(
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
    [HttpGet("{id:int}/details")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [ActionName("GetArticle")]
    public async Task<ActionResult<ArticleDto>> GetArticle(int id)
    {
        var result = await articleService.GetByIdAsync(id);
        return result.Match<ActionResult, ArticleDto>(
            success: value => Ok(new { value }),
            failure: error => NotFound(new { message = error.Message })
        );
    }

    /// <summary>
    /// Get last articles published
    /// </summary>
    /// <param name="amount">last articles amount desired</param>
    /// <response code="200">data retrieved</response>
    /// <response code="404">Not Found</response>
    /// <response code="500">service unvalaible</response>
    /// <returns>Last articles published</returns>
    [HttpGet("get-last")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<List<ArticleMinimumDto>>> GetLast(int amount)
    {
        var result = await articleService.GetLast(amount);
        return result.Match<ActionResult, List<ArticleMinimumDto>>(
            success: value => Ok(new { value }),
            failure: error => NotFound(new { message = error.Message })
        );
    }

    /// <summary>
    /// Retrieves a paginated list of articles filtered by the specified category.
    /// </summary>
    /// <param name="categoryId">The unique identifier of the desired category.</param>
    /// <param name="pageNumber">The page number to retrieve, starting from 1.</param>
    /// <param name="pageSize">The number of articles to include per page.</param>
    /// <response code="200">The data was successfully retrieved.</response>
    /// <response code="404">No articles were found for the specified category.</response>
    /// <response code="500">An unexpected error occurred while processing the request.</response>
    /// <returns>
    /// A paginated result containing articles belonging to the specified category.
    /// Returns a 200 status code if data is successfully retrieved.
    /// Returns a 404 status code if no articles are found for the given category.
    /// Returns a 500 status code if an internal server error occurs.
    /// </returns>
    [HttpGet("get-by-category")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<PagedResultDto<ArticleMinimumDto>>> GetByCategory(int categoryId, int pageNumber = 1, int pageSize = 10)
    {
        var result = await articleService.GetByCategory(categoryId, pageNumber, pageSize);
        return result.Match<ActionResult, PagedResultDto<ArticleMinimumDto>>(
            success: value => Ok(new { value }),
            failure: error => NotFound(new { message = error.Message })
        );
    }
}
