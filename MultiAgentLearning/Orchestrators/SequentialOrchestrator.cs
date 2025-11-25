using MultiAgentLearning.Core;

namespace MultiAgentLearning.Orchestrators;

/// <summary>
/// Sequential orchestration pattern - agents process in order
/// </summary>
public class SequentialOrchestrator : IOrchestrator
{
    private readonly List<IAgent> _agents;

    public SequentialOrchestrator(IEnumerable<IAgent> agents)
    {
        _agents = agents.ToList();
        
        if (_agents.Count == 0)
        {
            throw new ArgumentException("At least one agent is required", nameof(agents));
        }
    }

    public async Task<AgentMessage> ExecuteAsync(
        string input, 
        CancellationToken cancellationToken = default)
    {
        Console.WriteLine($"\n=== Sequential Orchestration Started ===");
        Console.WriteLine($"Input: {input[..Math.Min(100, input.Length)]}...\n");
        
        var message = new AgentMessage { Content = input, Sender = "User" };
        
        for (int i = 0; i < _agents.Count; i++)
        {
            var agent = _agents[i];
            Console.WriteLine($"[Step {i + 1}/{_agents.Count}] {agent.Name} ({agent.Role})");
            
            message = await agent.ProcessAsync(message, cancellationToken);
            
            Console.WriteLine($"Output preview: {message.Content[..Math.Min(150, message.Content.Length)]}...\n");
        }

        Console.WriteLine($"=== Sequential Orchestration Completed ===\n");
        return message;
    }
}
