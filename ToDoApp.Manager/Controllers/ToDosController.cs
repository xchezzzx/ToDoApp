using Dapr.Client;
using Microsoft.AspNetCore.Mvc;
using ToDoApp.Contracts;
using ToDoApp.Manager.Models;

namespace ToDoApp.Manager.Controllers;

[Route("todos")]
[ApiController]
public class TodosController(DaprClient daprClient) : ControllerBase
{
    private readonly DaprClient _daprClient = daprClient;

    [HttpGet("{id:Guid}")]
    public async Task<IActionResult> GetById([FromRoute] Guid id, CancellationToken ct)
    {
        const string accessorAppId = "todoapp-accessor";

        try
        {
            var todo = await _daprClient.InvokeMethodAsync<TodoDto>(
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

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateTodoRequest request, CancellationToken ct)
    {
        if (string.IsNullOrWhiteSpace(request.Title))
            return BadRequest("Title is required.");
        
        var message = new TodoCreatedMessage
        {
            Id = Guid.NewGuid(),
            Title = request.Title,
            Description = request.Description,
            IsCompleted = false,
            CreatedAt = DateTime.UtcNow
        };

        await _daprClient.PublishEventAsync(
            "pubsub",
            "todos",
            message,
            cancellationToken: ct
            );

        return Accepted(new { message.Id });
    }
}
