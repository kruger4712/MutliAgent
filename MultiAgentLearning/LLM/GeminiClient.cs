using Mscc.GenerativeAI;

namespace MultiAgentLearning.LLM;

/// <summary>
/// Implementation of Gemini client using Google's Generative AI SDK
/// </summary>
public class GeminiClient : IGeminiClient
{
    private readonly string _apiKey;
    private readonly string _modelName;
    private readonly GoogleAI _googleAI;

    public GeminiClient(string apiKey, string modelName = "gemini-2.0-flash-exp")
    {
        _apiKey = apiKey ?? throw new ArgumentNullException(nameof(apiKey));
        _modelName = modelName;
        _googleAI = new GoogleAI(apiKey: _apiKey);
    }

    public async Task<string> GenerateResponseAsync(
        string systemPrompt,
        string userPrompt,
        CancellationToken cancellationToken = default)
    {
        try
        {
            // Get the generative model
            var model = _googleAI.GenerativeModel(model: _modelName);

            // Combine system prompt and user prompt
            // Gemini doesn't have a separate system instruction in the chat,
            // so we prepend it to the user message
            var combinedPrompt = $"{systemPrompt}\n\n{userPrompt}";

            // Generate content
            var response = await model.GenerateContent(combinedPrompt);

            // Extract text from response
            if (response?.Text != null)
            {
                return response.Text;
            }

            // Fallback if no text in response
            return "No response generated from the model.";
        }
        catch (Exception ex)
        {
            throw new InvalidOperationException(
                $"Error calling Gemini API: {ex.Message}", ex);
        }
    }
}
