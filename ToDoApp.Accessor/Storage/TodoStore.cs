using ToDoApp.Accessor.Models;

namespace ToDoApp.Accessor.Storage;

public class TodoStore
{
    private static readonly Dictionary<Guid, TodoDto> _todos = [];

    public static TodoDto? GetById(Guid id)
        => _todos.TryGetValue(id, out var todo) ? todo : null;

    public static Guid SeedOne()
    {
        var id = Guid.Parse("11111111-1111-1111-1111-111111111111");
        var todo = new TodoDto
        {
            Id = id,
            Title = "Sample Todo",
            Description = "This is a sample todo item.",
            IsCompleted = false,
            CreatedAt = DateTime.UtcNow
        };
        _todos[id] = todo;
 
        return id;
    }
}
