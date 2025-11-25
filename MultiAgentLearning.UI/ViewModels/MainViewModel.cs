using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using MultiAgentLearning.Agents;
using MultiAgentLearning.Core;
using MultiAgentLearning.LLM;
using MultiAgentLearning.Orchestrators;
using MultiAgentLearning.UI.Models;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Windows;

namespace MultiAgentLearning.UI.ViewModels;

/// <summary>
/// Main ViewModel for the application window
/// </summary>
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

            // Update statuses during processing (simulated for now)
            _ = Task.Run(async () =>
            {
                await Task.Delay(500);
                Application.Current.Dispatcher.Invoke(() =>
                {
                    AgentStatuses[0].UpdateStatus(AgentStatus.Processing, 50);
                });

                await Task.Delay(1000);
                Application.Current.Dispatcher.Invoke(() =>
                {
                    AgentStatuses[0].UpdateStatus(AgentStatus.Completed, 100);
                    AgentStatuses[1].UpdateStatus(AgentStatus.Processing, 50);
                });

                await Task.Delay(1000);
                Application.Current.Dispatcher.Invoke(() =>
                {
                    AgentStatuses[1].UpdateStatus(AgentStatus.Completed, 100);
                    AgentStatuses[2].UpdateStatus(AgentStatus.Processing, 50);
                });
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
            ResultText = $"Error: {ex.Message}\n\nStack Trace:\n{ex.StackTrace}";
            StatusMessage = "Error occurred";

            // Mark agents as error
            foreach (var agent in AgentStatuses)
            {
                if (agent.Status == AgentStatus.Processing || agent.Status == AgentStatus.NotStarted)
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
            Clipboard.SetText(ResultText);
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
