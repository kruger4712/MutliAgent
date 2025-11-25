# UI Quick Start Guide

## Running the WPF Application

### Prerequisites
- .NET 8.0 SDK installed
- Visual Studio 2022 (or later) with WPF workload
- Gemini API key

### First-Time Setup

1. **Configure Your API Key**

   Using User Secrets (Recommended):
   ```bash
   cd MultiAgentLearning.UI
   dotnet user-secrets set "Gemini:ApiKey" "your-actual-api-key-here"
   ```

   Or using Environment Variable:
   ```powershell
   $env:GEMINI_API_KEY = "your-actual-api-key-here"
   ```

2. **Build the Solution**
   ```bash
   dotnet build
   ```

3. **Run the Application**
   
   From Command Line:
   ```bash
   dotnet run --project MultiAgentLearning.UI\MultiAgentLearning.UI.csproj
   ```

   From Visual Studio:
   - Set `MultiAgentLearning.UI` as the startup project
   - Press F5 to run with debugging
   - Or Ctrl+F5 to run without debugging

## Using the Application

### Main Window Overview

```
???????????????????????????????????????????????????????????????
?  Multi-Agent Learning System                          [?][?][×]?
???????????????????????????????????????????????????????????????
?  [Orchestration Pattern]                                     ?
?     ? Sequential    ? Concurrent                            ?
?                                                               ?
?  [Your Question]                                             ?
?  Enter your question or prompt...                            ?
?                                                               ?
?                                           [Ask Question]      ?
?                                                               ?
?  [Agent Processing]                                          ?
?  ? Analyzer Agent         Not Started    [??????????]       ?
?  ? Researcher Agent       Not Started    [??????????]       ?
?  ? Synthesizer Agent      Not Started    [??????????]       ?
?                                                               ?
?  [Results]                                                   ?
?  (Results will appear here after processing)                 ?
?                                                               ?
?  [Copy to Clipboard] [Clear]                                 ?
?                                                               ?
?  Status: Ready                                API: Connected ?
???????????????????????????????????????????????????????????????
```

### Step-by-Step Usage

#### 1. Choose Orchestration Mode
- **Sequential**: Agents process one after another (Analyzer ? Researcher ? Synthesizer)
- **Concurrent**: All agents process in parallel

Click the radio button for your preferred mode.

#### 2. Enter Your Question
Type or paste your question in the text area. For example:
- "What is artificial intelligence?"
- "Explain quantum computing to a beginner"
- "What are the benefits of meditation?"

#### 3. Ask the Question
Click the **"Ask Question"** button (or press Ctrl+Enter when implemented).

The button will change to **"Processing..."** while agents work.

#### 4. Watch Agent Progress
The Agent Processing panel shows real-time status:
- **Gray dot** (Not Started): Agent is waiting
- **Orange dot** (Processing): Agent is actively working
- **Green dot** (Completed): Agent finished successfully
- **Red dot** (Error): Agent encountered an error

Progress bars show:
- Indeterminate animation while processing
- 100% when completed

#### 5. View Results
The results area displays:
- **Final Result**: Synthesized output from all agents
- **From**: The final agent that produced the output
- **Content**: The actual response
- **Metadata**: Processing time, orchestration mode, token usage

#### 6. Actions
- **Copy to Clipboard**: Copy the results to clipboard for use elsewhere
- **Clear**: Clear all input and results to start fresh

## Example Scenarios

### Scenario 1: Quick Analysis
1. Select "Sequential"
2. Type: "What is machine learning?"
3. Click "Ask Question"
4. Wait 10-30 seconds
5. Read the comprehensive analysis in Results
6. Click "Copy to Clipboard" to save

### Scenario 2: Concurrent Processing
1. Select "Concurrent"
2. Type: "Compare Python and JavaScript"
3. Click "Ask Question"
4. Notice all agents start simultaneously
5. Results appear when synthesis completes

### Scenario 3: Multiple Queries
1. After first query completes, click "Clear"
2. Enter new question
3. Click "Ask Question"
4. Repeat as needed

## Troubleshooting

### Error: "API key not found"
**Solution**: Configure your API key using user secrets or environment variable (see First-Time Setup).

### Application Won't Start
**Solution**: 
1. Check .NET 8.0 SDK is installed: `dotnet --version`
2. Verify project builds: `dotnet build`
3. Check for compilation errors

### "Processing..." Never Completes
**Possible Causes**:
- Network connection issues
- API rate limiting
- Invalid API key

**Solution**:
1. Check your internet connection
2. Verify API key is valid
3. Check console output for error messages
4. Wait a few minutes and try again

### Buttons Are Disabled
**Cause**: "Ask Question" button is disabled when:
- Question text is empty
- Processing is in progress

**Solution**: Ensure you've entered a question and wait for current processing to complete.

### Results Don't Appear
**Solution**:
1. Check the Status bar for error messages
2. Look at Agent Processing panel - if agents show "Error", there was an issue
3. Try a simpler question
4. Check API key configuration

## Keyboard Shortcuts (Planned)

| Shortcut | Action |
|----------|--------|
| **Ctrl+Enter** | Submit question |
| **Ctrl+C** | Copy results (when focused) |
| **Ctrl+N** | New/Clear query |
| **Escape** | Cancel processing |
| **F1** | Help |

## Tips for Best Results

### Question Quality
- **Be specific**: "Explain machine learning algorithms used in image recognition" is better than "What is AI?"
- **Provide context**: "For a beginner" or "Technical explanation" helps agents tailor responses
- **Ask one thing**: Multiple questions may confuse the agents

### Performance Tips
- **Sequential mode** takes longer but provides more coherent results
- **Concurrent mode** is faster but may have less integrated output
- Longer questions take more processing time
- Complex topics require more tokens (may cost more)

## UI Features Reference

### Orchestration Pattern
- Locked during processing (cannot change mid-query)
- Default: Sequential
- Setting persists within session

### Question Input
- Multi-line support (Enter key adds new line)
- Max 5,000 characters recommended
- Placeholder text disappears when typing

### Agent Processing Panel
- Real-time status updates
- Color-coded indicators
- Progress bars with animation
- Can't interact with individual agents (view-only)

### Results Display
- Read-only text area
- Scrollable for long content
- Monospace font (Consolas) for better readability
- Preserves formatting

### Status Bar
- Left side: Current operation status
- Right side: API connection indicator
- Updates in real-time

## Advanced Usage

### Using with Different Models
The UI currently uses `gemini-2.0-flash-exp`. To change:
1. Edit `App.xaml.cs`
2. Modify the `GeminiClient` instantiation:
   ```csharp
   services.AddSingleton<IGeminiClient>(sp => 
       new GeminiClient(apiKey, "gemini-pro")); // Different model
   ```

### Monitoring Token Usage
Token usage appears in the Results metadata section after each query.

### Batch Processing
1. Prepare questions in a text file
2. Copy/paste each question
3. Process one at a time
4. Copy results to clipboard after each
5. Paste into your analysis document

## Getting Help

### Documentation
- [UI Design Specification](UI_DESIGN.md)
- [Implementation Guide](IMPLEMENTATION_GUIDE.md)
- [XAML Reference](UI_XAML.md)
- [Main README](../../README.md)

### Common Questions
**Q: How long should processing take?**  
A: Typically 10-30 seconds, depending on question complexity and mode.

**Q: Can I stop processing once started?**  
A: Not currently - cancellation support is planned for future versions.

**Q: What's the difference between Sequential and Concurrent?**  
A: Sequential processes agents one at a time (more coherent), Concurrent runs them in parallel (faster).

**Q: Can I see individual agent outputs?**  
A: Not currently - the UI shows only the final synthesized result. Check console output for details.

**Q: Is my API key stored securely?**  
A: Yes, User Secrets stores keys outside the project directory and they're never committed to source control.

## Next Steps

After getting comfortable with the UI:
1. Experiment with different question types
2. Compare Sequential vs Concurrent results
3. Try complex multi-part questions
4. Monitor performance and token usage
5. Provide feedback for improvements

## Version Information

**UI Version**: 1.0  
**Framework**: WPF (.NET 8.0)  
**Last Updated**: 2024-01-XX

---

**Need more help?** Check the documentation or open an issue on GitHub!
