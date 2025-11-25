using MultiAgentLearning.LLM;

namespace MultiAgentLearning.Agents;

/// <summary>
/// Agent specialized in synthesizing information into final output
/// </summary>
public class SynthesizerAgent : BaseAgent
{
    protected override string SystemPrompt => 
        @"You are a synthesizer agent. Your role is to:
        1. Take all previous agent outputs
        2. Combine them into a coherent, comprehensive response
        3. Ensure consistency and remove redundancies
        4. Present the final answer in a user-friendly format
        
        Focus on clarity and completeness.";

    public SynthesizerAgent(IGeminiClient geminiClient) 
        : base("Synthesizer", "Synthesis Expert", geminiClient)
    {
    }
}
