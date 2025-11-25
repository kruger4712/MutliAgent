namespace MultiAgentLearning.Core;

/// <summary>
/// Core interface for all agents in the system.
/// Each agent is a specialized entity that can process messages.
/// </summary>
public interface IAgent
{
    /// <summary>
    /// Unique identifier for the agent
    /// </summary>
    string Name { get; }
    
    /// <summary>
    /// The agent's specialized role or expertise
    /// </summary>
    string Role { get; }
    
    /// <summary>
    /// Process an incoming message and return a response
    /// </summary>
    Task<AgentMessage> ProcessAsync(AgentMessage input, CancellationToken cancellationToken = default);
}
