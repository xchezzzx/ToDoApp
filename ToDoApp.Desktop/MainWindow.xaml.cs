using System.Windows;
using ToDoApp.Desktop.ViewModels;

namespace ToDoApp.Desktop;

/// <summary>
/// Interaction logic for MainWindow.xaml
/// </summary>
public partial class MainWindow : Window
{
	private readonly MainWindowViewModel _mainWindowViewModel;

	public MainWindow(MainWindowViewModel mainWindowViewModel)
	{
		InitializeComponent();
		_mainWindowViewModel = mainWindowViewModel;
		DataContext = _mainWindowViewModel;

		Loaded += MainWindow_Loaded;
	}

	private async void MainWindow_Loaded(object sender, RoutedEventArgs e)
	{
		await _mainWindowViewModel.LoadTodosASync();
	}

	private async void CreateButton_Click(object sender, RoutedEventArgs e)
	{
		await _mainWindowViewModel.CreateTodoAsync();
	}

	private async void RefreshButton_Click(object sender, RoutedEventArgs e)
	{
		await _mainWindowViewModel.LoadTodosASync();
	}
}