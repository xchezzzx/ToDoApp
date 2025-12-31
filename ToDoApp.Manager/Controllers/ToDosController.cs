using Dapr.Client;
using Microsoft.AspNetCore.Mvc;
using ToDoApp.Contracts;
using ToDoApp.Manager.Clients;
using ToDoApp.Manager.Models;

namespace ToDoApp.Manager.Controllers;

[ApiController]
[Route("api/[controller]")]
public sealed class TodosController(ITodoAccessorClient accessorClient, DaprClient daprClient) : ControllerBase
{
    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetById(Guid id, CancellationToken cancellationToken)
    {
        try
        {
            var todo = await accessorClient.GetByIdAsync(id, cancellationToken);
            return Ok(todo);
        }
        catch (InvocationException)
        {
            return NotFound();
        }
    }

    [HttpGet]
    public async Task<IActionResult> GetAll(CancellationToken cancellationToken)
    {
        var todos = await accessorClient.GetAllAsync(cancellationToken);
        return Ok(todos);
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateTodoRequest request, CancellationToken cancellationToken)
    {
        if (string.IsNullOrWhiteSpace(request.Title))
        {
            return BadRequest("Title is required.");
        }

        var msg = new TodoCreatedMessage
        {
            Id = Guid.NewGuid(),
            Title = request.Title,
            Description = request.Description,
            IsCompleted = false,
            CreatedAt = DateTime.UtcNow
        };

        await daprClient.PublishEventAsync("pubsub", "todos", msg, cancellationToken);

        return Accepted(new { msg.Id });
    }
}
