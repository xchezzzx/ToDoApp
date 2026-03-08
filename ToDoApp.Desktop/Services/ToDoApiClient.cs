using System.Net.Http;
using System.Net.Http.Json;
using ToDoApp.Contracts;

namespace ToDoApp.Desktop.Services;

public sealed class ToDoApiClient(HttpClient httpClient)
{
	private readonly HttpClient _httpClient = httpClient;

	public async Task<List<TodoDto>> GetAllAsync(CancellationToken ct = default)
	{
		var result = await _httpClient.GetFromJsonAsync<List<TodoDto>>("todos", ct);
		return result ?? [];
	}

	public async Task<TodoDto?> GetByIdAsync(Guid id, CancellationToken ct = default)
	{
		return await _httpClient.GetFromJsonAsync<TodoDto>($"todos/{id}", ct);
	}

	public async Task<Guid?> CreateAsync(string title, string?  description, CancellationToken ct = default)
	{
		var response = await _httpClient.PostAsJsonAsync("todos", new
		{
			title,
			description
		}, ct);

		if (!response.IsSuccessStatusCode)
			return null;

		var payload = await response.Content.ReadFromJsonAsync<CreateTodoResponse>(cancellationToken:  ct);
		return payload?.Id ?? Guid.Empty;
	}

	public sealed class CreateTodoResponse
	{
		public Guid Id {  get; set; }
	}
}
