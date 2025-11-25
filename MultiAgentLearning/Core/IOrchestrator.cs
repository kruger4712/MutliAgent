namespace MultiAgentLearning.Core;

/// <summary>
/// Coordinates multiple agents to accomplish a task
/// </summary>
public interface IOrchestrator
{
    /// <summary>
    /// Execute the orchestration with the given input
    /// </summary>
    Task<AgentMessage> ExecuteAsync(string input, CancellationToken cancellationToken = default);
}
