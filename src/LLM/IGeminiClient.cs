namespace MultiAgentLearning.LLM;

/// <summary>
/// Interface for interacting with Google's Gemini LLM
/// </summary>
public interface IGeminiClient
{
    /// <summary>
    /// Generate a response from Gemini given system and user prompts
    /// </summary>
    Task<string> GenerateResponseAsync(
        string systemPrompt, 
        string userPrompt, 
        CancellationToken cancellationToken = default);
}
