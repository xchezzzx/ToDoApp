using System.Net;
using Dapr.Client;
using ToDoApp.Contracts;

namespace ToDoApp.Manager.Clients;

public sealed class TodoAccessorClient(DaprClient daprClient) : ITodoAccessorClient
{
    private const string AppId = "todoapp-accessor";

    public Task<TodoDto?> GetByIdAsync(Guid id, CancellationToken cancellationToken)
    {
        return daprClient.InvokeMethodAsync<TodoDto?>(HttpMethod.Get, AppId, $"todos/{id}", cancellationToken);
    }

    public Task<IEnumerable<TodoDto>> GetAllAsync(CancellationToken cancellationToken)
    {
        return daprClient.InvokeMethodAsync<IEnumerable<TodoDto>>(HttpMethod.Get, AppId, "todos", cancellationToken);
    }
}