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
    /// Retrieves a paginated list of articles based on a search term.
    /// </summary>
    /// <param name="pageNumber">The page number to retrieve, starting from 1.</param>
    /// <param name="pageSize">The number of articles to include per page.</param>
    /// <param name="searchTerm">The keyword or term provided by the client to filter articles.</param>
    /// <response code="200">The paginated list of articles was successfully retrieved.</response>
    /// <response code="404">No articles were found for the specified page.</response>
    /// <response code="500">An internal server error occurred while processing the request.</response>
    /// <returns>
    /// A paginated result containing articles belonging to the specified term.
    /// - A 200 status code with the paginated list of articles matching the search term.
    /// - A 404 status code if no articles are found for the given search term.
    /// - A 500 status code if there is a server error.
    /// </returns>
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
             failure: error => NotFound(new { message = Error.Alert, errors = error.Message })
        );
    }

    /// <summary>
    /// Retrieves a paginated list of articles.
    /// </summary>
    /// <param name="pageNumber">The page number to retrieve, starting from 1.</param>
    /// <param name="pageSize">The number of articles to include per page.</param>
    /// <response code="200">The paginated list of articles was successfully retrieved.</response>
    /// <response code="404">No articles were found for the specified page.</response>
    /// <response code="500">An internal server error occurred while processing the request.</response>
    /// <returns>
    /// A paginated result containing articles.
    /// - A 200 status code with the paginated list of articles if successful.
    /// - A 404 status code if no articles are found.
    /// - A 500 status code if there is a server error.
    /// </returns>
    [HttpGet("index")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<PagedResultDto<ArticleMinimumDto>>> GetArticles(int pageNumber = 1, int pageSize = 10)
    {
        var result = await articleService.GetAllAsync(pageNumber, pageSize);

        return result.Match<ActionResult, PagedResultDto<ArticleMinimumDto>>(
             success: value => Ok(new { value }),
             failure: error => NotFound(new { message = Error.Alert, errors = error.Message })
        );
    }

    /// <summary>
    /// Retrieves an article by its unique identifier.
    /// </summary>
    /// <param name="id">The unique identifier of the desired article.</param>
    /// <response code="200">The article was successfully retrieved.</response>
    /// <response code="404">The requested article does not exist.</response>
    /// <response code="500">An internal server error occurred while processing the request.</response>
    /// <returns>
    /// The article by the provided id.
    /// - A 200 status code with the article data if found.
    /// - A 404 status code if the article is not found.
    /// - A 500 status code if there is a server error.
    /// </returns>
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
            failure: error => NotFound(new { message = Error.Alert, errors = error.Message })
        );
    }

    /// <summary>
    /// Retrieves the most recently published articles up to the specified amount.
    /// </summary>
    /// <param name="amount">
    /// The number of recent articles to retrieve. Must be a positive integer.
    /// </param>
    /// <response code="200">The requested data was successfully retrieved.</response>
    /// <response code="404">No articles were found matching the criteria.</response>
    /// <response code="500">An internal server error occurred while processing the request.</response>
    /// <returns>
    /// A list of the most recently published articles in descending order of publication date.
    /// Returns a 200 status code if articles are successfully retrieved.
    /// Returns a 404 status code if no articles are found.
    /// Returns a 500 status code if an unexpected error occurs.
    /// </returns>
    [HttpGet("index-last")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<List<ArticleMinimumDto>>> GetLast(int amount)
    {
        var result = await articleService.GetLast(amount);
        return result.Match<ActionResult, List<ArticleMinimumDto>>(
            success: value => Ok(new { value }),
            failure: error => NotFound(new { message = Error.Alert, errors = error.Message })
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
    [HttpGet("index-by-category")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<PagedResultDto<ArticleMinimumDto>>> GetByCategory(int categoryId, int pageNumber = 1, int pageSize = 10)
    {
        var result = await articleService.GetByCategory(categoryId, pageNumber, pageSize);
        return result.Match<ActionResult, PagedResultDto<ArticleMinimumDto>>(
            success: value => Ok(new { value }),
            failure: error => NotFound(new { message = Error.Alert, errors = error.Message })
        );
    }
}
