using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using MultiAgentLearning.LLM;
using MultiAgentLearning.UI.ViewModels;
using System.Windows;

namespace MultiAgentLearning.UI;

/// <summary>
/// Interaction logic for App.xaml
/// </summary>
public partial class App : Application
{
    private readonly IHost _host;

    public App()
    {
        _host = Host.CreateDefaultBuilder()
            .ConfigureAppConfiguration((context, config) =>
            {
                config.AddUserSecrets<App>();
                config.AddEnvironmentVariables();
            })
            .ConfigureServices((context, services) =>
            {
                // Configuration - Get API key
                var apiKey = context.Configuration["Gemini:ApiKey"]
                    ?? Environment.GetEnvironmentVariable("GEMINI_API_KEY")
                    ?? throw new InvalidOperationException(
                        "API key not found. Set it using:\n" +
                        "  dotnet user-secrets set \"Gemini:ApiKey\" \"your-key\"\n" +
                        "OR set environment variable:\n" +
                        "  $env:GEMINI_API_KEY = \"your-key\"");

                // Register LLM client
                services.AddSingleton<IGeminiClient>(sp => new GeminiClient(apiKey));
                
                // Register ViewModels
                services.AddTransient<MainViewModel>();
                
                // Register Views
                services.AddTransient<MainWindow>();
            })
            .Build();
    }

    protected override async void OnStartup(StartupEventArgs e)
    {
        await _host.StartAsync();

        var mainWindow = _host.Services.GetRequiredService<MainWindow>();
        mainWindow.Show();

        base.OnStartup(e);
    }

    protected override async void OnExit(ExitEventArgs e)
    {
        await _host.StopAsync();
        _host.Dispose();

        base.OnExit(e);
    }
}

