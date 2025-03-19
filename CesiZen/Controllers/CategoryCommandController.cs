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
                new { name = dto.Name, message = result.Info.Message }, dto),
            failure: error => BadRequest(new { message = error.Message })
        );
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update([FromBody] CategoryDto dto)
    {
        var result = await categoryService.Update(dto);

        return result.Match<IActionResult>(
            success: () => Ok(new { result.Info.Message }),
            failure: error => BadRequest(new { message = error.Message })
        );
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(string id)
    {
        var result = await categoryService.Delete(id);

        return result.Match<IActionResult>(
            success: () => Ok(new { result.Info.Message }),
            failure: error => BadRequest(new { message = error.Message })
        );
    }
}
