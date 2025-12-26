using Dapr.Client;
using Microsoft.AspNetCore.Mvc;

namespace ToDoApp.Manager.Controllers;

[Route("api/[controller]")]
[ApiController]
public class ToDosController : ControllerBase
{
    private readonly DaprClient _daprClient;
    public ToDosController(DaprClient daprClient)
    {
        _daprClient = daprClient;
    }
}
