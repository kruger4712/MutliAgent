using MultiAgentLearning.Core;
using MultiAgentLearning.LLM;

namespace MultiAgentLearning.Agents;

/// <summary>
/// Base implementation for agents using LLM-based processing
/// </summary>
public abstract class BaseAgent : IAgent
{
    protected readonly IGeminiClient _geminiClient;
    
    public string Name { get; }
    public string Role { get; }
    
    /// <summary>
    /// The system prompt that defines this agent's behavior
    /// </summary>
    protected abstract string SystemPrompt { get; }

    protected BaseAgent(string name, string role, IGeminiClient geminiClient)
    {
        Name = name;
        Role = role;
        _geminiClient = geminiClient;
    }

    public virtual async Task<AgentMessage> ProcessAsync(
        AgentMessage input, 
        CancellationToken cancellationToken = default)
    {
        Console.WriteLine($"[{Name}] Processing message from {input.Sender ?? "User"}");
        
        var response = await _geminiClient.GenerateResponseAsync(
            SystemPrompt,
            input.Content,
            cancellationToken);

        return new AgentMessage
        {
            Content = response,
            Sender = Name,
            Metadata = new Dictionary<string, object>
            {
                ["Role"] = Role,
                ["ProcessedAt"] = DateTime.UtcNow,
                ["InputLength"] = input.Content.Length,
                ["OutputLength"] = response.Length
            }
        };
    }
}
