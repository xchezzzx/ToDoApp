using ToDoApp.Contracts;

namespace ToDoApp.Manager.Clients;

public interface ITodoAccessorClient
{
    Task<TodoDto?> GetByIdAsync(Guid id, CancellationToken cancellationToken);
    Task<IEnumerable<TodoDto>> GetAllAsync(CancellationToken cancellationToken);
}