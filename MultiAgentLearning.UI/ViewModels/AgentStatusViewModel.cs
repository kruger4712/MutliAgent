using CommunityToolkit.Mvvm.ComponentModel;
using MultiAgentLearning.UI.Models;

namespace MultiAgentLearning.UI.ViewModels;

/// <summary>
/// ViewModel for displaying the status of an individual agent
/// </summary>
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

    /// <summary>
    /// Updates the status and progress of the agent
    /// </summary>
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
