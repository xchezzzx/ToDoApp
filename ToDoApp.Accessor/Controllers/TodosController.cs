using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ToDoApp.Accessor.Data;

namespace ToDoApp.Accessor.Controllers;

[Route("todos")]
[ApiController]
public sealed class TodosController : ControllerBase
{
    private readonly TodoDbContext _dbContext;
    public TodosController(TodoDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    [HttpGet("{id:Guid}")]
    public async Task<IActionResult> GetTodoById([FromRoute] Guid id, CancellationToken ct)
    {
        var todo = await _dbContext.Todos.AsNoTracking()
            .Where(x => x.Id == id)
            .Select(x => new
            {
                x.Id,
                x.Title,
                x.Description,
                x.IsCompleted,
                x.CreatedAt
            })
            .SingleOrDefaultAsync(ct);

        return todo is null ? NotFound() : Ok(todo);
    }
}
