using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ToDoApp.Accessor.Data;
using ToDoApp.Contracts;

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
            .Select(x => new TodoDto
            {
                Id = x.Id,
                Title = x.Title,
                Description = x.Description,
                IsCompleted = x.IsCompleted,
                CreatedAt = x.CreatedAt
            })
            .SingleOrDefaultAsync(ct);

        return todo is null ? NotFound() : Ok(todo);
    }

    // get all todos
    [HttpGet]
    public async Task<IActionResult> GetAllTodos(CancellationToken ct)
    {
        var todos = await _dbContext.Todos.AsNoTracking()
            .Select(x => new TodoDto
            {
                Id = x.Id,
                Title = x.Title,
                Description = x.Description,
                IsCompleted = x.IsCompleted,
                CreatedAt = x.CreatedAt
            })
            .ToListAsync(ct);

        return todos.Count == 0 ? NotFound() : Ok(todos);
    }
}
