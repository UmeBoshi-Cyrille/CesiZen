using CesiZen.Domain.Interface;
using Microsoft.AspNetCore.Mvc;

namespace CesiZen.Api.Controllers;

public class UserController : Controller
{
    private readonly IUserCommandService commandService;
    private readonly IUserQueryService queryService;
    private readonly ILogger logger;

    public UserController(
        IUserCommandService commandService,
        IUserQueryService queryService,
        ILogger logger)
    {
        this.commandService = commandService;
        this.queryService = queryService;
        this.logger = logger;
    }

    public IActionResult Index()
    {
        return View();
    }
}
