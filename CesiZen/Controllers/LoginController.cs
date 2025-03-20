using CesiZen.Domain.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace CesiZen.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public abstract class LoginController : ControllerBase
{
    protected readonly INotifier notifier;
    protected readonly IObserver observer;

    public LoginController(INotifier notifier, IObserver observer)
    {
        this.notifier = notifier;
        this.observer = observer;
    }
}
