namespace MultiAgentLearning.Core;

/// <summary>
/// Represents a message passed between agents in the system
/// </summary>
public record AgentMessage
{
    /// <summary>
    /// The content/payload of the message
    /// </summary>
    public required string Content { get; init; }
    
    /// <summary>
    /// The agent or source that sent this message
    /// </summary>
    public string? Sender { get; init; }
    
    /// <summary>
    /// When this message was created
    /// </summary>
    public DateTime Timestamp { get; init; } = DateTime.UtcNow;
    
    /// <summary>
    /// Additional metadata for context passing between agents
    /// </summary>
    public Dictionary<string, object> Metadata { get; init; } = new();
}
