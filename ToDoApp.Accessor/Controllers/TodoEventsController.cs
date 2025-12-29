using Dapr;
using Microsoft.AspNetCore.Mvc;
using ToDoApp.Accessor.Data;
using ToDoApp.Accessor.Models;
using ToDoApp.Contracts;

namespace ToDoApp.Accessor.Controllers;

[ApiController]
public class TodoEventsController : ControllerBase
{
    private readonly TodoDbContext _dbContext;

    public TodoEventsController(TodoDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    [Topic("pubsub", "todos")]
    [HttpPost("todos-created")]
    public async Task<IActionResult> HandleTodoCreatedAsync([FromBody] TodoCreatedMessage message, CancellationToken ct)
    {
        var ifExists = await _dbContext.Todos.FindAsync(new object[] { message.Id }, ct);
        if (ifExists is not null)
        {
            return Conflict($"Todo with Id {message.Id} already exists.");
        }

        var entity = new TodoEntity
        {
            Id = message.Id,
            Title = message.Title,
            Description = message.Description,
            IsCompleted = message.IsCompleted,
            CreatedAt = message.CreatedAt
        };

        _dbContext.Todos.Add(entity);
        await _dbContext.SaveChangesAsync(ct);
        return Ok();
    }
}
