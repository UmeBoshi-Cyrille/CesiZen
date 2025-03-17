using CesiZen.Domain.BusinessResult;
using CesiZen.Domain.DataTransfertObject;
using CesiZen.Domain.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace CesiZen.Api.Controllers;

[ApiController]
[Route("/api/[controller]")]
public class CategoryCommandController : ControllerBase
{
    private readonly ICategoryCommandService categoryService;

    public CategoryCommandController(
        ICategoryCommandService categoryService)
    {
        this.categoryService = categoryService;
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CategoryDto dto)
    {
        var result = categoryService.Insert(dto).Result;

        return result.Match<ActionResult>(
            success: () => CreatedAtAction(
                nameof(CategoryQueryController.GetCategory),
                nameof(CategoryQueryController),
                new { name = dto.Name }, dto),
            failure: error => BadRequest(error)
        );
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update([FromBody] CategoryDto dto)
    {
        var result = await categoryService.Update(dto);

        return result.Match<IActionResult>(
            success: () => Ok("Ok"),
            failure: error => BadRequest(error)
        );
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(string id)
    {
        var result = await categoryService.Delete(id);

        return result.Match<IActionResult>(
            success: () => Ok("Ok"),
            failure: error => BadRequest(error)
        );
    }
}
