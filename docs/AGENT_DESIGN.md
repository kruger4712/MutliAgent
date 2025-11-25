# Agent Design Guide

## What Makes a Good Agent?

A well-designed agent is:
1. **Specialized** - Has a clear, focused role
2. **Autonomous** - Can operate independently
3. **Cooperative** - Works well with other agents
4. **Reliable** - Produces consistent results

## Agent Anatomy

### Core Components

```csharp
public class MyAgent : BaseAgent
{
    // 1. Identity
    public MyAgent(IGeminiClient client) 
        : base("MyAgent", "Specific Role", client) { }
    
    // 2. Behavior
    protected override string SystemPrompt => "Instructions...";
    
    // 3. Processing (optional override)
    public override async Task<AgentMessage> ProcessAsync(...) { }
}
```

## Designing System Prompts

### Prompt Structure Template

```
You are a [ROLE] agent. Your role is to:
1. [PRIMARY TASK]
2. [SECONDARY TASK]
3. [ADDITIONAL RESPONSIBILITIES]

[BEHAVIORAL GUIDELINES]
[OUTPUT FORMAT INSTRUCTIONS]
```

### Example: Good vs. Bad Prompts

? **Bad Prompt:**
```
"You are helpful."
```

? **Good Prompt:**
```
"You are a code review agent. Your role is to:
1. Identify potential bugs and security issues
2. Suggest performance improvements
3. Check for code style consistency

Focus on actionable feedback. Prioritize security over style.
Output format: List issues by severity (Critical, High, Medium, Low)."
```

## Agent Specialization Examples

### 1. Validator Agent
**Purpose:** Check correctness and quality

```csharp
protected override string SystemPrompt => 
    @"You are a validator agent. Review the input for:
    - Factual accuracy
    - Logical consistency
    - Completeness
    
    Respond with: APPROVED or list of issues to fix.";
```

### 2. Formatter Agent
**Purpose:** Transform output format

```csharp
protected override string SystemPrompt => 
    @"You are a formatter agent. Take the input and:
    - Convert to markdown format
    - Add proper headings and structure
    - Create bullet points for lists
    - Ensure readability
    
    Preserve all information, only change format.";
```

## Agent Communication Patterns

### Passing Context

Agents can pass context through metadata:

```csharp
return new AgentMessage
{
    Content = response,
    Metadata = new Dictionary<string, object>
    {
        ["ConfidenceScore"] = 0.95,
        ["SourcesUsed"] = new[] { "doc1", "doc2" },
        ["NeedsHumanReview"] = false
    }
};
```

## Advanced Agent Patterns

### 1. Agent with Tools

```csharp
public class SearchAgent : BaseAgent
{
    private readonly ISearchService _search;
    
    public SearchAgent(IGeminiClient client, ISearchService search) 
        : base("Searcher", "Research", client)
    {
        _search = search;
    }
    
    public override async Task<AgentMessage> ProcessAsync(...)
    {
        // Extract search query from input
        var query = ExtractQuery(input.Content);
        
        // Use tool
        var results = await _search.SearchAsync(query);
        
        // Enhance prompt with results
        var enhancedPrompt = $"{input.Content}\n\nSearch results:\n{results}";
        
        // Call LLM with enhanced context
        return await base.ProcessAsync(
            input with { Content = enhancedPrompt }, 
            cancellationToken);
    }
}
```

### 2. Conditional Agent

```csharp
public class RouterAgent : BaseAgent
{
    public override async Task<AgentMessage> ProcessAsync(...)
    {
        // Analyze input to determine routing
        var analysis = await AnalyzeIntent(input);
        
        return new AgentMessage
        {
            Content = analysis.Response,
            Metadata = new Dictionary<string, object>
            {
                ["NextAgent"] = analysis.SuggestedAgent,
                ["Confidence"] = analysis.Confidence
            }
        };
    }
}
```

## Testing Agents

### Unit Test Example

```csharp
[Fact]
public async Task AnalyzerAgent_ShouldExtractKeyTopics()
{
    // Arrange
    var mockGemini = new Mock<IGeminiClient>();
    mockGemini
        .Setup(x => x.GenerateResponseAsync(It.IsAny<string>(), It.IsAny<string>(), default))
        .ReturnsAsync("Topics: AI, Machine Learning, Neural Networks");
    
    var agent = new AnalyzerAgent(mockGemini.Object);
    var input = new AgentMessage { Content = "Tell me about AI" };
    
    // Act
    var result = await agent.ProcessAsync(input);
    
    // Assert
    Assert.Contains("AI", result.Content);
    Assert.Equal("Analyzer", result.Sender);
}
```

## Best Practices

### DO:
- ? Give agents clear, specific instructions
- ? Define expected output format
- ? Include examples in prompts when helpful
- ? Log agent inputs/outputs for debugging
- ? Use metadata to pass structured data

### DON'T:
- ? Make agents too general-purpose
- ? Create overlapping agent responsibilities
- ? Ignore error handling
- ? Forget to handle edge cases
- ? Make prompts too long (token limits!)

## Iterative Improvement

1. **Start Simple:** Basic system prompt
2. **Test:** Run with various inputs
3. **Analyze:** What works? What fails?
4. **Refine:** Update prompt based on results
5. **Measure:** Track quality metrics
6. **Repeat:** Continuous improvement

Document your iterations in code comments!
