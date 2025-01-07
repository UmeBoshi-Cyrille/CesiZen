using CesiZen.Domain.Interface;
using Microsoft.AspNetCore.Mvc;

namespace CesiZen.Api.Controllers;

public class LoginController : Controller
{
    private readonly ILoginCommandService commandService;
    private readonly ILoginQueryService queryService;
    private readonly ILogger logger;

    public LoginController(
        ILoginCommandService commandService,
        ILoginQueryService queryService,
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
