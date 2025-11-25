namespace MultiAgentLearning.UI.Models;

/// <summary>
/// Result from processing a query through the agent system
/// </summary>
public class ProcessingResult
{
    /// <summary>
    /// Main content of the result
    /// </summary>
    public string Content { get; set; } = string.Empty;

    /// <summary>
    /// Additional metadata about the processing
    /// </summary>
    public Dictionary<string, object> Metadata { get; set; } = new();

    /// <summary>
    /// When the processing completed
    /// </summary>
    public DateTime Timestamp { get; set; } = DateTime.Now;

    /// <summary>
    /// How long the processing took
    /// </summary>
    public TimeSpan ProcessingTime { get; set; }

    /// <summary>
    /// Whether the processing was successful
    /// </summary>
    public bool IsSuccess { get; set; }

    /// <summary>
    /// Error message if processing failed
    /// </summary>
    public string? ErrorMessage { get; set; }
}
