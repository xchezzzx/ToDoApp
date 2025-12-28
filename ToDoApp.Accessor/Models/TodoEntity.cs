namespace ToDoApp.Accessor.Models;

public sealed class TodoEntity
{
    public Guid Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string? Description { get; set; }
    public bool IsCompleted { get; set; }
    public DateTime? CreatedAt { get; set; }
}
