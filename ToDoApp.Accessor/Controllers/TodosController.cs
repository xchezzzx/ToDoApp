using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ToDoApp.Accessor.Models;
using ToDoApp.Accessor.Storage;

namespace ToDoApp.Accessor.Controllers;

[Route("todos")]
[ApiController]
public class TodosController : ControllerBase
{
    [HttpGet("{id:Guid}")]
    public ActionResult<TodoDto> GetTodoById([FromRoute] Guid id)
    {
        var todo = TodoStore.GetById(id);
        if (todo is null)
            return NotFound();
        
        return Ok(todo);
    }
}
