# UI Implementation Guide

## Getting Started

This guide provides step-by-step instructions for implementing the WPF UI for the Multi-Agent Learning System.

## Prerequisites

- Visual Studio 2022 (or later) with WPF workload
- .NET 8.0 SDK or later
- Existing Multi-Agent Learning System console application

## Project Setup

### Step 1: Create WPF Project

```bash
# Navigate to solution directory
cd C:\Users\a_dam\source\repos\kruger4712\MutliAgent

# Create new WPF project
dotnet new wpf -n MultiAgentLearning.UI -f net8.0-windows

# Add project to solution
dotnet sln add MultiAgentLearning.UI/MultiAgentLearning.UI.csproj

# Add reference to core project
cd MultiAgentLearning.UI
dotnet add reference ../src/MultiAgentLearning.csproj
```

### Step 2: Install Required NuGet Packages

```bash
# MVVM Toolkit
dotnet add package CommunityToolkit.Mvvm --version 8.3.2

# Dependency Injection
dotnet add package Microsoft.Extensions.DependencyInjection --version 8.0.0
dotnet add package Microsoft.Extensions.Hosting --version 8.0.0

# Configuration support (for API keys)
dotnet add package Microsoft.Extensions.Configuration --version 8.0.0
dotnet add package Microsoft.Extensions.Configuration.UserSecrets --version 8.0.0
```

### Step 3: Update Project File

Edit `MultiAgentLearning.UI.csproj`:

```xml
<Project Sdk="Microsoft.NET.Sdk">

  <PropertyGroup>
    <OutputType>WinExe</OutputType>
    <TargetFramework>net8.0-windows</TargetFramework>
    <Nullable>enable</Nullable>
    <UseWPF>true</UseWPF>
    <UserSecretsId>aspnet-MultiAgentLearning-UI</UserSecretsId>
  </PropertyGroup>

  <ItemGroup>
    <PackageReference Include="CommunityToolkit.Mvvm" Version="8.3.2" />
    <PackageReference Include="Microsoft.Extensions.DependencyInjection" Version="8.0.0" />
    <PackageReference Include="Microsoft.Extensions.Hosting" Version="8.0.0" />
    <PackageReference Include="Microsoft.Extensions.Configuration" Version="8.0.0" />
    <PackageReference Include="Microsoft.Extensions.Configuration.UserSecrets" Version="8.0.0" />
  </ItemGroup>

  <ItemGroup>
    <ProjectReference Include="..\src\MultiAgentLearning.csproj" />
  </ItemGroup>

</Project>
```

## Project Structure

Create the following folder structure:

```
MultiAgentLearning.UI/
??? App.xaml
??? App.xaml.cs
??? Views/
?   ??? MainWindow.xaml
?   ??? MainWindow.xaml.cs
?   ??? Controls/
?       ??? AgentStatusControl.xaml
?       ??? AgentStatusControl.xaml.cs
??? ViewModels/
?   ??? MainViewModel.cs
?   ??? AgentStatusViewModel.cs
?   ??? ViewModelBase.cs
??? Models/
?   ??? OrchestrationMode.cs
?   ??? AgentStatus.cs
?   ??? ProcessingResult.cs
??? Services/
?   ??? IDialogService.cs
?   ??? DialogService.cs
?   ??? IFileExportService.cs
?   ??? FileExportService.cs
??? Converters/
?   ??? StatusToColorConverter.cs
?   ??? BoolToVisibilityConverter.cs
?   ??? InverseBoolConverter.cs
??? Resources/
    ??? Styles/
    ?   ??? AppStyles.xaml
    ??? Colors/
        ??? ColorTheme.xaml
```

## Implementation Steps

### Phase 1: Core Infrastructure

#### 1.1 Create Enums and Models

**File: `Models/OrchestrationMode.cs`**

```csharp
namespace MultiAgentLearning.UI.Models;

public enum OrchestrationMode
{
    Sequential,
    Concurrent
}
```

**File: `Models/AgentStatus.cs`**

```csharp
namespace MultiAgentLearning.UI.Models;

public enum AgentStatus
{
    NotStarted,
    Processing,
    Completed,
    Error
}
```

**File: `Models/ProcessingResult.cs`**

```csharp
namespace MultiAgentLearning.UI.Models;

public class ProcessingResult
{
    public string Content { get; set; } = string.Empty;
    public Dictionary<string, object> Metadata { get; set; } = new();
    public DateTime Timestamp { get; set; } = DateTime.Now;
    public TimeSpan ProcessingTime { get; set; }
    public bool IsSuccess { get; set; }
    public string? ErrorMessage { get; set; }
}
```

#### 1.2 Create ViewModels

**File: `ViewModels/ViewModelBase.cs`**

```csharp
using CommunityToolkit.Mvvm.ComponentModel;

namespace MultiAgentLearning.UI.ViewModels;

/// <summary>
/// Base class for all ViewModels
/// </summary>
public abstract class ViewModelBase : ObservableObject
{
}
```

**File: `ViewModels/AgentStatusViewModel.cs`**

```csharp
using CommunityToolkit.Mvvm.ComponentModel;
using MultiAgentLearning.UI.Models;

namespace MultiAgentLearning.UI.ViewModels;

public partial class AgentStatusViewModel : ViewModelBase
{
    [ObservableProperty]
    private string _agentName = string.Empty;

    [ObservableProperty]
    private AgentStatus _status = AgentStatus.NotStarted;

    [ObservableProperty]
    private int _progress;

    [ObservableProperty]
    private string _statusText = "Not Started";

    [ObservableProperty]
    private TimeSpan _elapsedTime;

    public AgentStatusViewModel(string agentName)
    {
        AgentName = agentName;
    }

    public void UpdateStatus(AgentStatus status, int progress = 0)
    {
        Status = status;
        Progress = progress;
        StatusText = status switch
        {
            AgentStatus.NotStarted => "Not Started",
            AgentStatus.Processing => "Processing...",
            AgentStatus.Completed => "Completed",
            AgentStatus.Error => "Error",
            _ => "Unknown"
        };
    }
}
```

**File: `ViewModels/MainViewModel.cs`**

```csharp
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using MultiAgentLearning.Agents;
using MultiAgentLearning.Core;
using MultiAgentLearning.LLM;
using MultiAgentLearning.Orchestrators;
using MultiAgentLearning.UI.Models;
using System.Collections.ObjectModel;
using System.Diagnostics;

namespace MultiAgentLearning.UI.ViewModels;

public partial class MainViewModel : ViewModelBase
{
    private readonly IGeminiClient _geminiClient;

    [ObservableProperty]
    private OrchestrationMode _selectedOrchestrationMode = OrchestrationMode.Sequential;

    [ObservableProperty]
    private string _questionText = string.Empty;

    [ObservableProperty]
    private string _resultText = string.Empty;

    [ObservableProperty]
    private string _statusMessage = "Ready";

    [ObservableProperty]
    private bool _isProcessing;

    [ObservableProperty]
    private bool _hasResults;

    public ObservableCollection<AgentStatusViewModel> AgentStatuses { get; }

    public MainViewModel(IGeminiClient geminiClient)
    {
        _geminiClient = geminiClient;

        // Initialize agent status view models
        AgentStatuses = new ObservableCollection<AgentStatusViewModel>
        {
            new AgentStatusViewModel("Analyzer Agent"),
            new AgentStatusViewModel("Researcher Agent"),
            new AgentStatusViewModel("Synthesizer Agent")
        };
    }

    [RelayCommand(CanExecute = nameof(CanAskQuestion))]
    private async Task AskQuestionAsync()
    {
        IsProcessing = true;
        HasResults = false;
        StatusMessage = "Processing your question...";
        ResultText = string.Empty;

        try
        {
            // Reset agent statuses
            foreach (var agent in AgentStatuses)
            {
                agent.UpdateStatus(AgentStatus.NotStarted);
            }

            // Create agents
            var analyzer = new AnalyzerAgent(_geminiClient);
            var researcher = new ResearcherAgent(_geminiClient);
            var synthesizer = new SynthesizerAgent(_geminiClient);

            // Create orchestrator
            IOrchestrator orchestrator = SelectedOrchestrationMode switch
            {
                OrchestrationMode.Sequential => new SequentialOrchestrator(
                    new IAgent[] { analyzer, researcher, synthesizer }),
                OrchestrationMode.Concurrent => new ConcurrentOrchestrator(
                    new IAgent[] { analyzer, researcher, synthesizer }),
                _ => throw new InvalidOperationException("Invalid orchestration mode")
            };

            // Start tracking
            var stopwatch = Stopwatch.StartNew();

            // Update statuses during processing
            _ = Task.Run(async () =>
            {
                while (IsProcessing)
                {
                    await Task.Delay(100);
                    // Update UI with progress simulation
                    // In a real implementation, you'd get actual progress from agents
                }
            });

            // Execute orchestration
            var result = await orchestrator.ExecuteAsync(QuestionText);

            stopwatch.Stop();

            // Mark all as completed
            foreach (var agent in AgentStatuses)
            {
                agent.UpdateStatus(AgentStatus.Completed, 100);
            }

            // Format results
            ResultText = FormatResult(result, stopwatch.Elapsed);
            HasResults = true;
            StatusMessage = "Completed successfully";
        }
        catch (Exception ex)
        {
            ResultText = $"Error: {ex.Message}";
            StatusMessage = "Error occurred";

            // Mark agents as error
            foreach (var agent in AgentStatuses)
            {
                if (agent.Status == AgentStatus.Processing)
                {
                    agent.UpdateStatus(AgentStatus.Error);
                }
            }
        }
        finally
        {
            IsProcessing = false;
        }
    }

    private bool CanAskQuestion()
    {
        return !string.IsNullOrWhiteSpace(QuestionText) && !IsProcessing;
    }

    [RelayCommand(CanExecute = nameof(CanCopyResults))]
    private void CopyResults()
    {
        if (!string.IsNullOrEmpty(ResultText))
        {
            System.Windows.Clipboard.SetText(ResultText);
            StatusMessage = "Results copied to clipboard";
        }
    }

    private bool CanCopyResults()
    {
        return HasResults && !string.IsNullOrEmpty(ResultText);
    }

    [RelayCommand(CanExecute = nameof(CanClearResults))]
    private void ClearResults()
    {
        ResultText = string.Empty;
        QuestionText = string.Empty;
        HasResults = false;
        StatusMessage = "Ready";

        foreach (var agent in AgentStatuses)
        {
            agent.UpdateStatus(AgentStatus.NotStarted);
        }
    }

    private bool CanClearResults()
    {
        return HasResults || !string.IsNullOrEmpty(QuestionText);
    }

    private string FormatResult(AgentMessage result, TimeSpan elapsed)
    {
        var formatted = $"=== FINAL RESULT ===\n\n";
        formatted += $"From: {result.Sender}\n\n";
        formatted += $"Content:\n{result.Content}\n\n";
        formatted += $"=== METADATA ===\n";
        formatted += $"Processing Time: {elapsed.TotalSeconds:F2}s\n";
        formatted += $"Orchestration Mode: {SelectedOrchestrationMode}\n";

        foreach (var (key, value) in result.Metadata)
        {
            formatted += $"{key}: {value}\n";
        }

        return formatted;
    }

    partial void OnQuestionTextChanged(string value)
    {
        AskQuestionCommand.NotifyCanExecuteChanged();
    }

    partial void OnIsProcessingChanged(bool value)
    {
        AskQuestionCommand.NotifyCanExecuteChanged();
        ClearResultsCommand.NotifyCanExecuteChanged();
    }

    partial void OnHasResultsChanged(bool value)
    {
        CopyResultsCommand.NotifyCanExecuteChanged();
        ClearResultsCommand.NotifyCanExecuteChanged();
    }
}
```

### Phase 2: UI Components

#### 2.1 Create Value Converters

**File: `Converters/StatusToColorConverter.cs`**

```csharp
using System.Globalization;
using System.Windows.Data;
using System.Windows.Media;
using MultiAgentLearning.UI.Models;

namespace MultiAgentLearning.UI.Converters;

public class StatusToColorConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        if (value is AgentStatus status)
        {
            return status switch
            {
                AgentStatus.NotStarted => new SolidColorBrush(Color.FromRgb(128, 128, 128)), // Gray
                AgentStatus.Processing => new SolidColorBrush(Color.FromRgb(255, 165, 0)),   // Orange
                AgentStatus.Completed => new SolidColorBrush(Color.FromRgb(40, 167, 69)),    // Green
                AgentStatus.Error => new SolidColorBrush(Color.FromRgb(220, 53, 69)),        // Red
                _ => new SolidColorBrush(Colors.Gray)
            };
        }
        return new SolidColorBrush(Colors.Gray);
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}
```

**File: `Converters/BoolToVisibilityConverter.cs`**

```csharp
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace MultiAgentLearning.UI.Converters;

public class BoolToVisibilityConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        if (value is bool boolValue)
        {
            return boolValue ? Visibility.Visible : Visibility.Collapsed;
        }
        return Visibility.Collapsed;
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        if (value is Visibility visibility)
        {
            return visibility == Visibility.Visible;
        }
        return false;
    }
}
```

**File: `Converters/InverseBoolConverter.cs`**

```csharp
using System.Globalization;
using System.Windows.Data;

namespace MultiAgentLearning.UI.Converters;

public class InverseBoolConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        if (value is bool boolValue)
        {
            return !boolValue;
        }
        return true;
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        if (value is bool boolValue)
        {
            return !boolValue;
        }
        return false;
    }
}
```

#### 2.2 Create Resource Dictionaries

**File: `Resources/Colors/ColorTheme.xaml`**

```xml
<ResourceDictionary xmlns="http://schemas.microsoft.com/winfx/2006/xaml/presentation"
                    xmlns:x="http://schemas.microsoft.com/winfx/2006/xaml">
    
    <!-- Light Theme Colors -->
    <SolidColorBrush x:Key="PrimaryBackgroundBrush" Color="#FFFFFF"/>
    <SolidColorBrush x:Key="SecondaryBackgroundBrush" Color="#F5F5F5"/>
    <SolidColorBrush x:Key="TextPrimaryBrush" Color="#212529"/>
    <SolidColorBrush x:Key="TextSecondaryBrush" Color="#6C757D"/>
    <SolidColorBrush x:Key="AccentBrush" Color="#007BFF"/>
    <SolidColorBrush x:Key="SuccessBrush" Color="#28A745"/>
    <SolidColorBrush x:Key="WarningBrush" Color="#FFA500"/>
    <SolidColorBrush x:Key="ErrorBrush" Color="#DC3545"/>
    <SolidColorBrush x:Key="BorderBrush" Color="#DEE2E6"/>
    
</ResourceDictionary>
```

**File: `Resources/Styles/AppStyles.xaml`**

```xml
<ResourceDictionary xmlns="http://schemas.microsoft.com/winfx/2006/xaml/presentation"
                    xmlns:x="http://schemas.microsoft.com/winfx/2006/xaml">
    
    <!-- Merge color theme -->
    <ResourceDictionary.MergedDictionaries>
        <ResourceDictionary Source="../Colors/ColorTheme.xaml"/>
    </ResourceDictionary.MergedDictionaries>

    <!-- GroupBox Style -->
    <Style TargetType="GroupBox">
        <Setter Property="Margin" Value="0,0,0,12"/>
        <Setter Property="Padding" Value="12"/>
        <Setter Property="BorderBrush" Value="{StaticResource BorderBrush}"/>
        <Setter Property="BorderThickness" Value="1"/>
        <Setter Property="FontSize" Value="16"/>
        <Setter Property="FontWeight" Value="SemiBold"/>
    </Style>

    <!-- TextBox Style -->
    <Style TargetType="TextBox">
        <Setter Property="Padding" Value="8"/>
        <Setter Property="FontSize" Value="14"/>
        <Setter Property="BorderBrush" Value="{StaticResource BorderBrush}"/>
        <Setter Property="BorderThickness" Value="1"/>
    </Style>

    <!-- Button Style -->
    <Style TargetType="Button">
        <Setter Property="Padding" Value="16,8"/>
        <Setter Property="FontSize" Value="14"/>
        <Setter Property="FontWeight" Value="Medium"/>
        <Setter Property="Background" Value="{StaticResource AccentBrush}"/>
        <Setter Property="Foreground" Value="White"/>
        <Setter Property="BorderThickness" Value="0"/>
        <Setter Property="Cursor" Value="Hand"/>
        <Style.Triggers>
            <Trigger Property="IsMouseOver" Value="True">
                <Setter Property="Opacity" Value="0.9"/>
            </Trigger>
            <Trigger Property="IsEnabled" Value="False">
                <Setter Property="Opacity" Value="0.5"/>
            </Trigger>
        </Style.Triggers>
    </Style>

    <!-- ProgressBar Style -->
    <Style TargetType="ProgressBar">
        <Setter Property="Height" Value="8"/>
        <Setter Property="Foreground" Value="{StaticResource AccentBrush}"/>
    </Style>

    <!-- StatusBar Style -->
    <Style TargetType="StatusBar">
        <Setter Property="Background" Value="{StaticResource SecondaryBackgroundBrush}"/>
        <Setter Property="BorderBrush" Value="{StaticResource BorderBrush}"/>
        <Setter Property="BorderThickness" Value="0,1,0,0"/>
        <Setter Property="Padding" Value="8"/>
    </Style>

</ResourceDictionary>
```

### Phase 3: Main Window

**File: `Views/MainWindow.xaml`** (See next section)

**File: `Views/MainWindow.xaml.cs`**

```csharp
using System.Windows;
using MultiAgentLearning.UI.ViewModels;

namespace MultiAgentLearning.UI.Views;

public partial class MainWindow : Window
{
    public MainWindow(MainViewModel viewModel)
    {
        InitializeComponent();
        DataContext = viewModel;
    }
}
```

### Phase 4: Application Startup

**File: `App.xaml.cs`**

```csharp
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using MultiAgentLearning.LLM;
using MultiAgentLearning.UI.ViewModels;
using MultiAgentLearning.UI.Views;
using System.Windows;

namespace MultiAgentLearning.UI;

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
                // Configuration
                var apiKey = context.Configuration["Gemini:ApiKey"]
                    ?? Environment.GetEnvironmentVariable("GEMINI_API_KEY")
                    ?? throw new InvalidOperationException("API key not configured");

                // Register services
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
```

## Next Steps

1. **Implement MainWindow.xaml** (see UI_XAML.md)
2. **Add Services** (DialogService, FileExportService)
3. **Test the Application**
4. **Add Additional Features**

## Common Issues & Solutions

### Issue: API Key Not Found

**Solution**: Configure user secrets:
```bash
cd MultiAgentLearning.UI
dotnet user-secrets init
dotnet user-secrets set "Gemini:ApiKey" "your-api-key-here"
```

### Issue: ViewModel Properties Not Updating

**Solution**: Ensure you're using `ObservableProperty` attribute or calling `OnPropertyChanged()`

### Issue: Commands Not Executing

**Solution**: Check `CanExecute` methods and call `NotifyCanExecuteChanged()` when conditions change

### Issue: Binding Errors in Output Window

**Solution**: 
- Verify property names match exactly
- Ensure DataContext is set correctly
- Check converter implementations

## Debugging Tips

1. **Enable Binding Diagnostics**:
   Add to App.xaml.cs `OnStartup`:
   ```csharp
   PresentationTraceSources.SetTraceLevel(binding, PresentationTraceLevel.High);
   ```

2. **Check Output Window**: Look for binding errors and exceptions

3. **Use Snoop/Live Visual Tree**: Visual Studio's Live Visual Tree tool is invaluable

4. **Breakpoints in Converters**: Add breakpoints to verify converter logic

## Performance Optimization

1. **Use Virtualization** for large collections
2. **Implement Lazy Loading** for heavy operations
3. **Freeze Resources** that don't change
4. **Use `x:Static`** instead of `x:Key` where possible
5. **Profile with Performance Profiler** in Visual Studio

## Deployment

### Build for Release

```bash
dotnet publish -c Release -r win-x64 --self-contained true -p:PublishSingleFile=true
```

### Create Installer (Optional)

Consider using:
- WiX Toolset
- Inno Setup
- MSIX packaging

## Additional Resources

- [WPF Documentation](https://learn.microsoft.com/en-us/dotnet/desktop/wpf/)
- [MVVM Toolkit Documentation](https://learn.microsoft.com/en-us/dotnet/communitytoolkit/mvvm/)
- [WPF Tutorial](https://wpf-tutorial.com/)
