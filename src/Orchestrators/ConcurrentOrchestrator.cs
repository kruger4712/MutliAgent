using MultiAgentLearning.Core;

namespace MultiAgentLearning.Orchestrators;

/// <summary>
/// Concurrent orchestration pattern - all agents process in parallel
/// </summary>
public class ConcurrentOrchestrator : IOrchestrator
{
    private readonly List<IAgent> _agents;

    public ConcurrentOrchestrator(IEnumerable<IAgent> agents)
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
        Console.WriteLine($"\n=== Concurrent Orchestration Started ===");
        Console.WriteLine($"Input: {input[..Math.Min(100, input.Length)]}...");
        Console.WriteLine($"Running {_agents.Count} agents in parallel...\n");
        
        var inputMessage = new AgentMessage { Content = input, Sender = "User" };
        
        // Run all agents concurrently
        var tasks = _agents.Select(agent => agent.ProcessAsync(inputMessage, cancellationToken)).ToList();
        var results = await Task.WhenAll(tasks);
        
        // Combine all results
        var combinedContent = string.Join("\n\n--- Next Agent Response ---\n\n", 
            results.Select(r => $"[{r.Sender}]: {r.Content}"));
        
        Console.WriteLine($"=== Concurrent Orchestration Completed ===\n");
        
        return new AgentMessage
        {
            Content = combinedContent,
            Sender = "ConcurrentOrchestrator",
            Metadata = new Dictionary<string, object>
            {
                ["AgentCount"] = _agents.Count,
                ["Agents"] = _agents.Select(a => a.Name).ToList()
            }
        };
    }
}
