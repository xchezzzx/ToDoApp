namespace ToDoApp.Contracts;

public sealed class TodoCreatedMessage
{
    public Guid Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string? Description { get; set; }
    public bool IsCompleted { get; init; }
    public DateTime CreatedAt { get; set; }
}
