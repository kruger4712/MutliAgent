# Multi-Agent System Architecture

## Overview

This system demonstrates multi-agent orchestration patterns using C# and Google's Gemini LLM.

## Core Components

### 1. Agents (`src/Agents/`)

**BaseAgent** - Abstract base class for all agents
- Handles LLM communication
- Manages message processing
- Tracks metadata

**Specialized Agents:**
- `AnalyzerAgent` - Analyzes input, extracts key information
- `ResearcherAgent` - Researches and expands on topics
- `SynthesizerAgent` - Combines results into coherent output

### 2. Orchestrators (`src/Orchestrators/`)

Coordinate multiple agents to accomplish tasks.

**SequentialOrchestrator:**
```
User Input ? Agent 1 ? Agent 2 ? Agent 3 ? Final Output
```

**ConcurrentOrchestrator:**
```
            ?? Agent 1 ??
User Input  ?? Agent 2 ??? Combine ? Final Output
            ?? Agent 3 ??
```

### 3. Core Interfaces (`src/Core/`)

- `IAgent` - Contract for all agents
- `IOrchestrator` - Contract for orchestration patterns
- `AgentMessage` - Message structure for inter-agent communication

### 4. LLM Integration (`src/LLM/`)

- `IGeminiClient` - Interface for LLM calls
- `GeminiClient` - Gemini-specific implementation

## Design Decisions

### Why Interfaces?

Interfaces allow you to:
1. Swap orchestration patterns easily
2. Test with mock agents
3. Support multiple LLM providers
4. Add new agent types without changing existing code

### Why Record Types for Messages?

`AgentMessage` is a record because:
- Immutable by default (safer for concurrent operations)
- Value-based equality
- Built-in ToString() for logging
- Concise syntax with `init` properties

### Metadata Strategy

Each message carries metadata to:
- Track processing pipeline
- Measure performance
- Debug issues
- Provide context for downstream agents

## Data Flow

### Sequential Pattern
```
1. User provides input
2. Create initial AgentMessage
3. For each agent in sequence:
   a. Agent processes previous message
   b. Agent creates new message with response
   c. New message becomes input for next agent
4. Return final message
```

### Concurrent Pattern
```
1. User provides input
2. Create initial AgentMessage
3. Broadcast message to all agents simultaneously
4. Wait for all agents to complete
5. Aggregate all responses
6. Return combined message
```

## Extensibility Points

### Adding New Agents
1. Create class inheriting from `BaseAgent`
2. Override `SystemPrompt` property
3. Optionally override `ProcessAsync` for custom logic

### Adding New Orchestration Patterns
1. Implement `IOrchestrator`
2. Define agent coordination logic
3. Register in Program.cs

### Supporting Multiple LLMs
1. Implement `IGeminiClient` for new provider
2. Inject into agents via constructor
3. No other code changes needed

## Future Enhancements

- [ ] Add tool/function calling to agents
- [ ] Implement handoff orchestration pattern
- [ ] Add conversation state management
- [ ] Create group chat with manager agent
- [ ] Add streaming responses
- [ ] Implement retry and error handling
- [ ] Add cost tracking and token counting
- [ ] Create agent performance metrics
