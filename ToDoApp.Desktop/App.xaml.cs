using Microsoft.Extensions.DependencyInjection;
using System.Net.Http;
using System.Windows;
using ToDoApp.Desktop.Services;
using ToDoApp.Desktop.ViewModels;

namespace ToDoApp.Desktop;

/// <summary>
/// Interaction logic for App.xaml
/// </summary>
public partial class App : Application
{
	public IServiceProvider Services { get; private set; } = null!;

	protected override void OnStartup(StartupEventArgs e)
	{
		var serviceCollection = new ServiceCollection();

		serviceCollection.AddSingleton(new HttpClient
		{
			BaseAddress = new Uri("http://localhost:5084")
		});

		serviceCollection.AddSingleton<ToDoApiClient>();
		serviceCollection.AddSingleton<MainWindowViewModel>();
		serviceCollection.AddSingleton<MainWindow>();

		Services = serviceCollection.BuildServiceProvider();

		var mainWindow = Services.GetRequiredService<MainWindow>();
		mainWindow.Show();

		base.OnStartup(e);
	}
}
