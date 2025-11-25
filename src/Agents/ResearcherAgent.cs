using MultiAgentLearning.LLM;

namespace MultiAgentLearning.Agents;

/// <summary>
/// Agent specialized in researching and expanding on topics
/// </summary>
public class ResearcherAgent : BaseAgent
{
    protected override string SystemPrompt => 
        @"You are a researcher agent. Your role is to:
        1. Take analyzed information and expand on key topics
        2. Provide relevant context and background
        3. Identify knowledge gaps that need clarification
        4. Suggest related areas to explore
        
        Be thorough but concise in your research output.";

    public ResearcherAgent(IGeminiClient geminiClient) 
        : base("Researcher", "Research Specialist", geminiClient)
    {
    }
}
