using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using ToDoApp.Contracts;
using ToDoApp.Desktop.Services;

namespace ToDoApp.Desktop.ViewModels;

public sealed class MainWindowViewModel : INotifyPropertyChanged
{
	private readonly ToDoApiClient _apiClient;

	private string _title = string.Empty;
	private string? _description;
	private bool _isBusy;

	public ObservableCollection<TodoDto> Todos { get; } = new();

	public string TitleInput
	{
		get => _title;
		set
		{
			_title = value;
			OnPropertyChanged();
		}
	}

	public string? DescriptionInput
	{
		get => _description;
		set
		{
			_description = value;
			OnPropertyChanged();
		}
	}

	public bool IsBusy
	{
		get => _isBusy;
		set
		{
			_isBusy = value;
			OnPropertyChanged();
		}
	}

	public MainWindowViewModel(ToDoApiClient apiClient)
	{
		_apiClient = apiClient;
	}

	public async Task LoadTodosASync()
	{
		IsBusy = true;
		try
		{
			Todos.Clear();
			var items = await _apiClient.GetAllAsync();
			foreach (var item in items)
				Todos.Add(item);
		}
		finally
		{
			IsBusy = false;
		}
	}

	public async Task CreateTodoAsync()
	{
		if (string.IsNullOrWhiteSpace(TitleInput))
			return;

		var id = await _apiClient.CreateAsync(_title, _description);

		if (id is not null)
		{
			TitleInput = string.Empty;
			DescriptionInput = string.Empty;
			await LoadTodosASync();
		}
	}

	public event PropertyChangedEventHandler? PropertyChanged;

	private void OnPropertyChanged([CallerMemberName] string? name = null)
		=> PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
}
