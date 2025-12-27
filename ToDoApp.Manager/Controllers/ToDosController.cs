using Dapr.Client;
using Microsoft.AspNetCore.Mvc;

namespace ToDoApp.Manager.Controllers;

[Route("todos")]
[ApiController]
public class ToDosController : ControllerBase
{
    private readonly DaprClient _daprClient;

    public ToDosController(DaprClient daprClient)
    {
        _daprClient = daprClient;
    }

    [HttpGet("{id:Guid}")]
    public async Task<IActionResult> GetById([FromRoute] Guid id, CancellationToken ct)
    {
        const string accessorAppId = "todoapp-accessor";

        try
        {
            var todo = await _daprClient.InvokeMethodAsync<object>(
                HttpMethod.Get,
                accessorAppId,
                $"todos/{id}",
                cancellationToken: ct
                );

            return Ok(todo);
        }
        catch (InvocationException ex) when (ex.Response is not null && (int)ex.Response.StatusCode == 404)
        {
            return NotFound();
        }
    }
}
