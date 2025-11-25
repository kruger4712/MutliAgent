# Multi-Agent Learning System

A C# learning project exploring multi-agent orchestration patterns using Google's Gemini LLM.

## ?? Purpose

This project helps developers:
- Understand multi-agent system architecture
- Learn different orchestration patterns
- Experiment with agent design
- Evaluate multi-agent approaches for production use

## ?? Quick Start

### Prerequisites
- .NET 8.0 SDK
- Visual Studio 2022 (for WPF UI) or any .NET IDE
- Google Gemini API key ([Get one here](https://makersuite.google.com/app/apikey))

### ? Recommended: WPF Desktop UI

**The easiest way to get started!**

1. **Clone the repository**
```bash
git clone https://github.com/kruger4712/MutliAgent.git
cd MutliAgent
```

2. **Set your API key using User Secrets**
```bash
cd MultiAgentLearning.UI
dotnet user-secrets set "Gemini:ApiKey" "your-api-key-here"
```

3. **Run the desktop application**
```bash
dotnet run --project MultiAgentLearning.UI\MultiAgentLearning.UI.csproj
```

**Or open in Visual Studio:**
- Set `MultiAgentLearning.UI` as startup project
- Press **F5** to run!

?? **Detailed instructions:** [UI Quick Start Guide](docs/UI/QUICK_START_UI.md)

---

### Alternative: Console Application

For lightweight usage or remote/SSH scenarios:

1. **Clone and navigate**
```bash
git clone https://github.com/kruger4712/MutliAgent.git
cd MutliAgent
```

2. **Set your API key**
```bash
# Windows PowerShell
$env:GEMINI_API_KEY = "your-api-key-here"

# Linux/Mac
export GEMINI_API_KEY=your-api-key-here
```

3. **Run the console application**
```bash
cd src
dotnet run
```

?? **See:** [Console Quick Start](docs/QUICK_START.md) | [How to Run Guide](HOW_TO_RUN.md)

## ?? Documentation

### Getting Started
- **[How to Run](HOW_TO_RUN.md)** - Choose UI or Console ? START HERE
- **[UI Quick Start](docs/UI/QUICK_START_UI.md)** - Using the WPF interface
- **[Getting Started](docs/GETTING_STARTED.md)** - Setup and first run
- **[Quick Start Guide](docs/QUICK_START.md)** - Console application guide

### Architecture & Design
- **[Architecture](docs/ARCHITECTURE.md)** - System design and components
- **[Patterns](docs/PATTERNS.md)** - Orchestration patterns explained
- **[Agent Design](docs/AGENT_DESIGN.md)** - Creating effective agents
- **[UI Design](docs/UI/UI_DESIGN.md)** - WPF interface specification

### Advanced Topics
- **[Lessons Learned](docs/LESSONS_LEARNED.md)** - Insights from experiments
- **[Cost Analysis](docs/COST_ANALYSIS.md)** - Token usage and optimization
- **[Production Considerations](docs/PRODUCTION_CONSIDERATIONS.md)** - Path to production

### UI Documentation
- **[UI README](docs/UI/README.md)** - UI documentation index
- **[UI Implementation Guide](docs/UI/IMPLEMENTATION_GUIDE.md)** - Building the WPF UI
- **[UI XAML Reference](docs/UI/UI_XAML.md)** - XAML markup guide

## ?? Project Structure

```
MutliAgent/
??? src/                        # Console application
?   ??? Core/                   # Core interfaces and types
?   ??? Agents/                 # Agent implementations
?   ??? Orchestrators/          # Orchestration patterns
?   ??? LLM/                    # Gemini client integration
?   ??? Program.cs              # Console entry point
??? MultiAgentLearning.UI/      # WPF Desktop UI ?
?   ??? Views/                  # XAML views
?   ??? ViewModels/             # MVVM ViewModels
?   ??? Models/                 # UI data models
?   ??? Converters/             # Value converters
?   ??? Resources/              # Styles and colors
??? docs/                       # Documentation
?   ??? UI/                     # UI-specific docs
?   ??? *.md                    # Other guides
??? HOW_TO_RUN.md              # Run instructions ?
??? README.md
```

## ?? User Interface Features

The WPF desktop application provides:
- ? **Modern UI** - Clean, professional interface
- ? **Visual Agent Tracking** - Real-time progress indicators
- ? **Pattern Selection** - Easy toggle between Sequential/Concurrent
- ? **Result Management** - Copy, clear, and export results
- ? **MVVM Architecture** - Clean separation of concerns
- ? **Status Indicators** - Color-coded agent states

### UI vs Console

| Feature | WPF UI | Console |
|---------|--------|---------|
| Visual Agent Tracking | ? | ? |
| Color-Coded Status | ? | ? |
| One-Click Copy | ? | ? |
| Remote/SSH Access | ? | ? |
| Resource Usage | Medium | Low |

**Recommendation:** Use WPF UI for learning and demos, Console for automation and remote access.

## ?? Implemented Patterns

### 1. Sequential Orchestration
Agents process in order: Analyzer ? Researcher ? Synthesizer

**Use when:** Clear dependencies between stages

### 2. Concurrent Orchestration
All agents process simultaneously in parallel

**Use when:** Independent analyses needed

## ?? Example Usage

### Using the WPF UI
1. Launch the application
2. Select orchestration pattern (Sequential or Concurrent)
3. Enter your question
4. Click "Ask Question"
5. Watch agents process in real-time
6. View results and copy to clipboard

### Programmatic Usage
```csharp
// Create agents
var analyzer = new AnalyzerAgent(geminiClient);
var researcher = new ResearcherAgent(geminiClient);
var synthesizer = new SynthesizerAgent(geminiClient);

// Choose pattern
var orchestrator = new SequentialOrchestrator(new[] 
{ 
    analyzer, researcher, synthesizer 
});

// Execute
var result = await orchestrator.ExecuteAsync("Explain quantum computing");
Console.WriteLine(result.Content);
```

## ?? Learning Path

1. **Week 1-2:** Understand basics, run the UI application
2. **Week 3-4:** Experiment with sequential vs concurrent patterns
3. **Week 5-6:** Explore the code, understand MVVM architecture
4. **Week 7-8:** Advanced features (tools, state management)
5. **Week 9-10:** Production considerations, apply to real scenarios

## ?? Metrics to Track

- Latency per pattern
- Token usage and costs
- Quality of outputs
- Error rates

Document your findings in `docs/LESSONS_LEARNED.md`!

## ?? Contributing

This is a learning project. Feel free to:
- Experiment with different patterns
- Create new agent types
- Improve the UI/UX
- Share insights in documentation
- Submit PRs with improvements

## ?? License

MIT License - see LICENSE file for details

## ?? Resources

- [Semantic Kernel Agent Framework](https://learn.microsoft.com/en-us/semantic-kernel/frameworks/agent/)
- [AI Agent Design Patterns](https://learn.microsoft.com/en-us/azure/architecture/ai-ml/guide/ai-agent-design-patterns)
- [Google Gemini API](https://ai.google.dev/)
- [WPF Documentation](https://learn.microsoft.com/en-us/dotnet/desktop/wpf/)
- [MVVM Toolkit](https://learn.microsoft.com/en-us/dotnet/communitytoolkit/mvvm/)

## ? Questions?

Open an issue or refer to the comprehensive docs in the `/docs` folder.

---

**Note:** This is a learning and experimentation project. For production use, review the [Production Considerations](docs/PRODUCTION_CONSIDERATIONS.md) guide.
