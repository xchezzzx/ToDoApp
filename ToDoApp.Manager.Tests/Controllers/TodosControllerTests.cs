using System.Net;
using Dapr.Client;
using Microsoft.AspNetCore.Mvc;
using Moq;
using ToDoApp.Contracts;
using ToDoApp.Manager.Clients;
using ToDoApp.Manager.Controllers;
using ToDoApp.Manager.Models;
using Xunit;

namespace ToDoApp.Manager.Tests.Controllers;

public sealed class TodosControllerTests
{
    [Fact]
    public async Task GetById_WhenTodoExists_ReturnsOkWithTodo()
    {
        var ct = CancellationToken.None;
        var id = Guid.NewGuid();

        var expected = new TodoDto
        {
            Id = id,
            Title = "t",
            Description = "d",
            IsCompleted = false,
            CreatedAt = DateTime.UtcNow
        };

        var accessorMock = new Mock<ITodoAccessorClient>(MockBehavior.Strict);
        accessorMock
            .Setup(x => x.GetByIdAsync(id, ct))
            .ReturnsAsync(expected);

        var daprClientMock = new Mock<DaprClient>(MockBehavior.Strict);

        var controller = new TodosController(accessorMock.Object, daprClientMock.Object);

        var result = await controller.GetById(id, ct);

        var ok = Assert.IsType<OkObjectResult>(result);
        var payload = Assert.IsType<TodoDto>(ok.Value);
        Assert.Equal(expected.Id, payload.Id);
        Assert.Equal(expected.Title, payload.Title);

        accessorMock.VerifyAll();
        daprClientMock.VerifyNoOtherCalls();
    }

    [Fact]
    public async Task GetById_WhenAccessorReturns404_ReturnsNotFound()
    {
        var ct = CancellationToken.None;
        var id = Guid.NewGuid();

        var accessorMock = new Mock<ITodoAccessorClient>(MockBehavior.Strict);
        accessorMock
            .Setup(x => x.GetByIdAsync(id, ct))
            .ThrowsAsync(new InvocationException("todoapp-accessor", $"todos/{id}", null));

        var daprClientMock = new Mock<DaprClient>(MockBehavior.Strict);

        var controller = new TodosController(accessorMock.Object, daprClientMock.Object);

        var result = await controller.GetById(id, ct);

        Assert.IsType<NotFoundResult>(result);

        accessorMock.VerifyAll();
        daprClientMock.VerifyNoOtherCalls();
    }

    [Fact]
    public async Task GetAll_ReturnsOkWithTodos()
    {
        var ct = CancellationToken.None;

        var expected = new[]
        {
            new TodoDto { Id = Guid.NewGuid(), Title = "a" },
            new TodoDto { Id = Guid.NewGuid(), Title = "b" }
        };

        var accessorMock = new Mock<ITodoAccessorClient>(MockBehavior.Strict);
        accessorMock
            .Setup(x => x.GetAllAsync(ct))
            .ReturnsAsync(expected);

        var daprClientMock = new Mock<DaprClient>(MockBehavior.Strict);

        var controller = new TodosController(accessorMock.Object, daprClientMock.Object);

        var result = await controller.GetAll(ct);

        var ok = Assert.IsType<OkObjectResult>(result);
        var payload = Assert.IsAssignableFrom<IEnumerable<TodoDto>>(ok.Value);
        Assert.Equal(2, payload.Count());

        accessorMock.VerifyAll();
        daprClientMock.VerifyNoOtherCalls();
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("   ")]
    public async Task Create_WhenTitleMissing_ReturnsBadRequest(string? title)
    {
        var ct = CancellationToken.None;
        var request = new CreateTodoRequest { Title = title ?? string.Empty, Description = "d" };

        var accessorMock = new Mock<ITodoAccessorClient>(MockBehavior.Strict);
        var daprClientMock = new Mock<DaprClient>(MockBehavior.Strict);

        var controller = new TodosController(accessorMock.Object, daprClientMock.Object);

        var result = await controller.Create(request, ct);

        var badRequest = Assert.IsType<BadRequestObjectResult>(result);
        Assert.Equal("Title is required.", badRequest.Value);

        accessorMock.VerifyNoOtherCalls();
        daprClientMock.VerifyNoOtherCalls();
    }

    [Fact]
    public async Task Create_WhenValid_PublishesEvent_AndReturnsAcceptedWithId()
    {
        var ct = CancellationToken.None;
        var request = new CreateTodoRequest { Title = "t", Description = "d" };

        TodoCreatedMessage? published = null;

        var accessorMock = new Mock<ITodoAccessorClient>(MockBehavior.Strict);

        var daprClientMock = new Mock<DaprClient>(MockBehavior.Strict);
        daprClientMock
            .Setup(x => x.PublishEventAsync(
                "pubsub",
                "todos",
                It.IsAny<TodoCreatedMessage>(),
                ct))
            .Callback<string, string, TodoCreatedMessage, CancellationToken>((_, _, msg, _) => published = msg)
            .Returns(Task.CompletedTask);

        var controller = new TodosController(accessorMock.Object, daprClientMock.Object);

        var result = await controller.Create(request, ct);

        var accepted = Assert.IsType<AcceptedResult>(result);
        Assert.NotNull(accepted.Value);

        var idProperty = accepted.Value!.GetType().GetProperty("Id");
        Assert.NotNull(idProperty);

        var returnedId = (Guid)idProperty!.GetValue(accepted.Value)!;
        Assert.NotEqual(Guid.Empty, returnedId);

        Assert.NotNull(published);
        Assert.Equal(returnedId, published!.Id);
        Assert.Equal(request.Title, published.Title);
        Assert.Equal(request.Description, published.Description);
        Assert.False(published.IsCompleted);
        Assert.True((DateTime.UtcNow - published.CreatedAt) < TimeSpan.FromSeconds(5));

        accessorMock.VerifyNoOtherCalls();
        daprClientMock.VerifyAll();
    }
}