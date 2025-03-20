using CesiZen.Domain.BusinessResult;
using CesiZen.Domain.DataTransfertObject;
using CesiZen.Domain.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace CesiZen.Api.Controllers;

[ApiController]
[Route("/api/[controller]")]
public class CategoryQueryController : ControllerBase
{
    private readonly ICategoryQueryService categoryService;

    public CategoryQueryController(ICategoryQueryService categoryService)
    {
        this.categoryService = categoryService;
    }

    [HttpGet("categories")]
    public async Task<ActionResult<PagedResult<CategoryRequestDto>>> GetCategories([FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 10)
    {
        var result = await categoryService.GetAllAsync(pageNumber, pageSize);

        return result.Match<ActionResult, PagedResult<CategoryRequestDto>>(
             success: value => Ok(new { value }),
             failure: error => BadRequest(new { message = error.Message })
        );
    }

    [HttpGet("category/{id}")]
    public async Task<ActionResult<CategoryRequestDto>> GetCategory(string id)
    {
        var result = await categoryService.GetByIdAsync(id);
        return result.Match<ActionResult, CategoryRequestDto>(
            success: value => Ok(new { value }),
            failure: error => BadRequest(new { message = error.Message })
        );
    }
}
