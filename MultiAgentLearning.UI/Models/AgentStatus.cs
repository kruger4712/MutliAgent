namespace MultiAgentLearning.UI.Models;

/// <summary>
/// Status of an individual agent during processing
/// </summary>
public enum AgentStatus
{
    /// <summary>
    /// Agent has not started processing yet
    /// </summary>
    NotStarted,

    /// <summary>
    /// Agent is currently processing
    /// </summary>
    Processing,

    /// <summary>
    /// Agent has completed successfully
    /// </summary>
    Completed,

    /// <summary>
    /// Agent encountered an error
    /// </summary>
    Error
}
