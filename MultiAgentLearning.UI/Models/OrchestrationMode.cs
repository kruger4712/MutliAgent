namespace MultiAgentLearning.UI.Models;

/// <summary>
/// Orchestration mode for multi-agent processing
/// </summary>
public enum OrchestrationMode
{
    /// <summary>
    /// Agents process one after another
    /// </summary>
    Sequential,

    /// <summary>
    /// Agents process in parallel
    /// </summary>
    Concurrent
}
