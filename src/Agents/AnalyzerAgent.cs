using MultiAgentLearning.LLM;

namespace MultiAgentLearning.Agents;

/// <summary>
/// Agent specialized in analyzing and extracting key information
/// </summary>
public class AnalyzerAgent : BaseAgent
{
    protected override string SystemPrompt => 
        @"You are an analyzer agent. Your role is to analyze input and identify:
        1. Key themes and topics
        2. Important entities (people, places, organizations)
        3. Overall sentiment and tone
        4. Any questions or action items
        
        Provide your analysis in a structured, clear format.";

    public AnalyzerAgent(IGeminiClient geminiClient) 
        : base("Analyzer", "Analysis Expert", geminiClient)
    {
    }
}
