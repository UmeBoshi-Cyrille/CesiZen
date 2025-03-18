using CesiZen.Domain.BusinessResult;
using CesiZen.Domain.DataTransfertObject;
using CesiZen.Domain.Interface;
using Microsoft.AspNetCore.Mvc;

namespace CesiZen.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class UserCommandController : ControllerBase
{
    private readonly IUserCommandService userCommandService;

    public UserCommandController(
        IUserCommandService userCommandService)
    {
        this.userCommandService = userCommandService;
    }

    [HttpPut("update")]
    public async Task<IActionResult> Update([FromBody] UserDto dto)
    {
        var result = await userCommandService.Update(dto);

        return result.Match<IActionResult>(
            success: () => Ok(new { result.Info.Message }),
            failure: error => BadRequest(new { error })
        );
    }

    [HttpDelete("delete/{id}")]
    public async Task<IActionResult> Delete(string id)
    {
        var result = await userCommandService.Delete(id);

        return result.Match<IActionResult>(
            success: () => Ok(new { result.Info.Message }),
            failure: error => BadRequest(new { error })
        );
    }
}
