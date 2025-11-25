# Getting Started with Multi-Agent Learning

This guide will help you set up and run your first multi-agent system.

## Prerequisites

- .NET 8.0 or later
- Google Gemini API key
- Visual Studio 2022 or VS Code

## Setup

### 1. Get Gemini API Key

1. Go to [Google AI Studio](https://makersuite.google.com/app/apikey)
2. Create a new API key
3. Copy the key for later use

### 2. Set Environment Variable

**Windows (PowerShell):**
```powershell
$env:GEMINI_API_KEY = "your-api-key-here"
```

**Windows (Command Prompt):**
```cmd
set GEMINI_API_KEY=your-api-key-here
```

**Linux/Mac:**
```bash
export GEMINI_API_KEY=your-api-key-here
```

### 3. Install Dependencies

```bash
cd src
dotnet add package Google.GenerativeAI --prerelease
```

### 4. Run the Application

```bash
dotnet run
```

## Your First Multi-Agent Interaction

1. Choose an orchestration pattern (Sequential or Concurrent)
2. Enter a prompt like: "Explain quantum computing"
3. Watch as agents process your request:
   - **Analyzer**: Identifies key topics
   - **Researcher**: Expands with details
   - **Synthesizer**: Combines into final answer

## What's Next?

- Read [ARCHITECTURE.md](ARCHITECTURE.md) to understand system design
- Explore [PATTERNS.md](PATTERNS.md) for orchestration patterns
- Check [AGENT_DESIGN.md](AGENT_DESIGN.md) to create custom agents

## Troubleshooting

**Issue: "GEMINI_API_KEY not set"**
- Ensure environment variable is set in your current terminal session

**Issue: NotImplementedException**
- The GeminiClient needs full implementation
- Install Google.GenerativeAI package
- See implementation notes in code comments
